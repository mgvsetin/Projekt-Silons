using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variables

    [SerializeField] private float moveSpeed;
    public bool attacking;
    private bool ignoreCollider;
    public Transform wayPoint1, wayPoint2;
    public Player player;
    private Transform wayPointTarget;
    private Transform target;

    public bool playerVisible;
    public float detectionValue;
    [SerializeField] private float maxDetectionValue;
    public EnemyManager enemyManager;
    public FieldOfView fov;
    public bool newDetectionValueSet;
    public DetectionBar detectionBar;
    [SerializeField] private float detectionSpeed;
    private bool onLastSeenPos;


    private void Awake()
    {
        wayPointTarget = wayPoint1;
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyManager = FindObjectOfType<EnemyManager>();
        fov = FindObjectOfType<FieldOfView>();
    }

    private void Update()
    {
        DetectingPlayer();
    }


    public void Move()
    {
        if (player.behindCover)
        {
            Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), player.GetComponent<CapsuleCollider2D>(), ignoreCollider = true);
        }
        else
        {
            Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), player.GetComponent<CapsuleCollider2D>(), ignoreCollider = false);
        }

        if(Vector3.Distance(transform.position, wayPoint1.position) < 0.75f)
        {
            wayPointTarget = wayPoint2;
            Flip();
        }

        if(Vector3.Distance(transform.position, wayPoint2.position) < 0.75f)
        {
            wayPointTarget = wayPoint1;
            Flip();
        }

            transform.position = Vector2.MoveTowards(transform.position, wayPointTarget.position, moveSpeed * Time.deltaTime);     
    }

    public void Chasing()
    {
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), player.GetComponent<CapsuleCollider2D>(), ignoreCollider = false);
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);  
    }

    private void DetectingPlayer()
    {

        if (playerVisible)
        {
            if (enemyManager.alarmed && !newDetectionValueSet && detectionValue < enemyManager.alarmedValue)
            {
                detectionValue = enemyManager.alarmedValue;
                newDetectionValueSet = true;
            }

            if (detectionValue < maxDetectionValue)
            {
                detectionValue += detectionSpeed * Time.deltaTime;
            }
        }
        //else if (detectionValue >= 0)
        //{
          //  detectionValue -= detectionSpeed * Time.deltaTime;
        //}

        detectionBar.SetDetectionValue(detectionValue);

    }

   public IEnumerator WaitBeforeInvestigating()
    {
        yield return new WaitForSeconds(0.5f);
        Investigating();
    }

    public void Investigating()
    {
        if (Vector3.Distance(transform.position, fov.lastSeenPos) > 0.75f)
        {
            transform.position = Vector2.MoveTowards(transform.position, fov.lastSeenPos, moveSpeed * Time.deltaTime);
            onLastSeenPos = true;
        }
        if(onLastSeenPos)
        {
            GameObject closestCover;
            GameObject[] covers = GameObject.FindGameObjectsWithTag("Cover");
            for(int i = 0; i > covers.Length; i++)
            {
                
            }

            if (detectionValue > 0)
            {
                detectionValue -= detectionSpeed * Time.deltaTime;
            }
        }
    }

}
