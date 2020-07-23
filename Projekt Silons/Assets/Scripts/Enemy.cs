using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variables

    [SerializeField] private float moveSpeed;
    [SerializeField] private float distance;
    private bool ignoreCollider;
    public Transform wayPoint1, wayPoint2;
    public Player player;
    private Transform wayPointTarget;
    private Transform target;


    private void Awake()
    {
        wayPointTarget = wayPoint1;
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (Vector3.Distance(transform.position, target.position) > distance)
        {
            Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), player.GetComponent<CapsuleCollider2D>(), ignoreCollider = true);

            if(Vector3.Distance(transform.position, wayPoint1.position) < 0.75f)
            {
                wayPointTarget = wayPoint2;
            }

            if(Vector3.Distance(transform.position, wayPoint2.position) < 0.75f)
            {
                wayPointTarget = wayPoint1;
            }

            transform.position = Vector2.MoveTowards(transform.position, wayPointTarget.position, moveSpeed * Time.deltaTime);
        }
        else if(player.behindCover == false)
        {
            Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), player.GetComponent<CapsuleCollider2D>(), ignoreCollider = false);
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            Debug.Log("Chasing Player");
        }
        
    }
}
