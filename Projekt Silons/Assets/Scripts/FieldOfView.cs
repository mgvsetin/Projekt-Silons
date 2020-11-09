using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public LayerMask groundMask;
    public List<Transform> visibleTargets = new List<Transform>();
    public Enemy enemy;
    public float meshResolution;
    public MeshFilter viewMeshFilter;
    private Mesh viewMesh;
    public Transform lastSeenPos;
    public GameObject lastSeenPosWaypointPrefab;
    public  GameObject lastSeenPosWaypointClone;
    public List<GameObject> lastSeenPosWaypoits;

    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        enemy = gameObject.GetComponentInParent<Enemy>();
        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisableTargets();
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(-Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), -Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    public void FindVisableTargets()
    {
        visibleTargets.Clear();
        Collider2D[] targetsInView = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInView.Length; i++)
        {
            Transform target = targetsInView[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(-transform.right, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    lastSeenPos = target;
                    lastSeenPosWaypointClone = Instantiate(lastSeenPosWaypointPrefab, lastSeenPos.position, Quaternion.identity);
                    lastSeenPosWaypoits.Add(lastSeenPosWaypointClone);
                    
                }
            }
        }

        if (visibleTargets.Count > 0)
        {
            enemy.playerVisible = true;
        }
        else
        {
            enemy.playerVisible = false;
            enemy.newDetectionValueSet = false;
        }
    }

    private void DrawFieldOfView()
    {
        int rayCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float rayAngleSize = viewAngle / rayCount;
        List<Vector3> viewPoints = new List<Vector3>();

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle /2 + rayAngleSize * i;
            Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * viewRadius, Color.red);
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for(int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if(i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 2;
                triangles[i * 3 + 2] = i + 1;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }
}
