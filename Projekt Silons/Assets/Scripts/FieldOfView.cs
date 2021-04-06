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

    //Calculating direction of angle
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(-Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), -Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    public void FindVisableTargets()
    {
        visibleTargets.Clear();
        //Getting targets in radius
        Collider2D[] targetsInView = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInView.Length; i++)
        {
            Transform target = targetsInView[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            //Cheking if player is new FOV
            if (Vector3.Angle(-transform.right, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                //Cheking if theres no obstacle between player and enemy
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    lastSeenPos = target;
                    lastSeenPosWaypointClone = Instantiate(lastSeenPosWaypointPrefab, lastSeenPos.position, Quaternion.identity);
                    lastSeenPosWaypoits.Add(lastSeenPosWaypointClone);
                }
            }
        }

        //Player is visible
        if (visibleTargets.Count > 0)
        {
            enemy.playerVisible = true;
        }
        //Player is not visible
        else
        {
            enemy.playerVisible = false;
            enemy.newDetectionValueSet = false;
        }
    }

    private void DrawFieldOfView()
    {
        int rayCount = Mathf.RoundToInt(viewAngle * meshResolution); //Number of rays in FOV
        float rayAngleSize = viewAngle / rayCount; //Angle between each ray
        List<Vector3> viewPoints = new List<Vector3>();

        for (int i = 0; i <= rayCount; i++)
        {
            float angle = transform.eulerAngles.z - viewAngle /2 + rayAngleSize * i; //Getting angle of each ray fired from enemy
            ViewCastInfo newViewCast = ViewCast(angle);  //Methode for checking if ray hit obstacle
            viewPoints.Add(newViewCast.point); //Add point from methode above
        }

        //Drawing Mesh from end points of rays

        int vertexCount = viewPoints.Count + 1; //number of end points of rays
        Vector3[] vertices = new Vector3[vertexCount]; //end points of rays
        int[] triangles = new int[(vertexCount - 2) * 3]; //triangles made from endpoints

        vertices[0] = Vector3.zero;

        //Making triangles from endpoints
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

        //Setting Mesh
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(float globalAngle) //Methode for checking if ray hit obstacle and adding end points of rays
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask)) //hit obstacle
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle); //end point of ray is where ray hit obstacle
        }
        else //didnt hit
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle); //end point of ray is on the viewRadius
        }
    }

    //ViewCastInfo is a struct, struct is a custom datatype that can hold more then one data type
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
