using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeadRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    private Vector2 rotationDir;
    private Quaternion lookingDir;
    private Enemy enemy;

    private void Start()
    {
        enemy = gameObject.GetComponentInParent(typeof(Enemy)) as Enemy;
    }
    void Update()
    {
        rotationDir = -(enemy.aiDestinationSetter.target.position - transform.position).normalized;
        var angle = Mathf.Atan2(rotationDir.y, rotationDir.x) * Mathf.Rad2Deg;
        lookingDir = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookingDir, Time.deltaTime * rotationSpeed);
    }
}
