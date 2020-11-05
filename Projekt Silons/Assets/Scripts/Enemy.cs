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
    public Player player;
    public Transform[] waypoints;
    private int randomWaypoint;
    private Vector3 target;
    private float waitTime;
    [SerializeField] private float startWaitTime;
    [SerializeField] private float startWaitTimeCover;

    public bool playerVisible;
    public float detectionValue;
    [SerializeField] private float maxDetectionValue;
    public EnemyManager enemyManager;
    public FieldOfView fov;
    public bool newDetectionValueSet;
    public DetectionBar detectionBar;
    [SerializeField] private float detectionSpeed;
    private bool onLastSeenPos;
    [SerializeField] private GameObject closestCover;
    [SerializeField] private float coverRayDistance;
    private GameObject tempClosestCover;


    private void Awake()
    {
        randomWaypoint = UnityEngine.Random.Range(0, waypoints.Length);
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        enemyManager = FindObjectOfType<EnemyManager>();
        fov = FindObjectOfType<FieldOfView>();
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), player.GetComponent<CapsuleCollider2D>(), ignoreCollider = true);
    }

    private void Update()
    {
        DetectingPlayer();
        if (target != Vector3.zero && Vector2.Distance(transform.position, target) > 0.75f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        }
    }


    public void Move()
    {
        target = waypoints[randomWaypoint].position;
        if (player.behindCover)
        {
            Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), player.GetComponent<CapsuleCollider2D>(), ignoreCollider = true);
        }
        else
        {
            Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), player.GetComponent<CapsuleCollider2D>(), ignoreCollider = false);
        }

        if (Vector3.Distance(transform.position, waypoints[randomWaypoint].position) < 0.80f)
        {
            if(waitTime <= 0)
            {
                randomWaypoint = UnityEngine.Random.Range(0, waypoints.Length);
                waitTime = startWaitTime;
                Flip();
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    public void Chasing()
    {
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), player.GetComponent<CapsuleCollider2D>(), ignoreCollider = false);
        transform.position = Vector2.MoveTowards(transform.position, fov.lastSeenPos, moveSpeed * Time.deltaTime);
    }

    private void Flip()
    {
        //if (transform.rotation == Quaternion.Euler(0, 180, 0) && transform.position.x < target.x)
        //{
        //transform.Rotate(0, 180, 0);
        //}

        //if (transform.rotation == Quaternion.Euler(0, 0, 0) && transform.position.x > target.x)
        //{
        //transform.Rotate(0, 180, 0);
        //}

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

        detectionBar.SetDetectionValue(detectionValue);

    }

    public IEnumerator WaitBeforeInvestigating()
    {
        target = Vector3.zero;
        yield return new WaitForSeconds(0.2f);
        Investigating();
    }

    public void Investigating()
    {
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), player.GetComponent<CapsuleCollider2D>(), ignoreCollider = true);

        if (!onLastSeenPos && Vector2.Distance(transform.position, fov.lastSeenPos) > 0.75f)
        {
            target = fov.lastSeenPos;
            onLastSeenPos = false;
        }
        if(Vector2.Distance(transform.position, fov.lastSeenPos) <= 3f)
        {
            onLastSeenPos = true;
        }
        if (onLastSeenPos)
        {
            GameObject[] covers = GameObject.FindGameObjectsWithTag("Cover");
            SortCovers(covers);
            closestCover = null;

            for (int i = 0; i < covers.Length; i++)
            {
                if (closestCover == null)
                {
                    if (transform.rotation == Quaternion.Euler(0, 0, 0) && transform.position.x > covers[i].transform.position.x)
                    {
                        closestCover = covers[i];
                    }

                    if (transform.rotation == Quaternion.Euler(0, 180, 0) && transform.position.x < covers[i].transform.position.x)
                    {
                        closestCover = covers[i];
                    }
                }
            }
            if(closestCover != null)
            {
                target = closestCover.transform.position;
                GetPlayerOutOfCover(closestCover);
            }
        }
        Debug.Log(closestCover);
        onLastSeenPos = false;
        DecreaseDetectionValue();

    }

    public GameObject[] SortCovers(GameObject[] unsortedCovers)
    {
        int min;
        GameObject temp;

        for(int i = 0; i < unsortedCovers.Length; i++)
        {
            min = i;
            for(int j = i + 1; j < unsortedCovers.Length; j++)
            {
                if(Vector2.Distance(transform.position, unsortedCovers[j].transform.position) < Vector2.Distance(transform.position, unsortedCovers[min].transform.position))
                {
                    min = j;
                }
            }

            if(min != i)
            {
                temp = unsortedCovers[i];
                unsortedCovers[i] = unsortedCovers[min];
                unsortedCovers[min] = temp;
            }
        }
        return unsortedCovers;
    }

    private void DecreaseDetectionValue()
    {
        if (detectionValue > 0)
        {
            detectionValue -= detectionSpeed * Time.deltaTime;
        }
    }

    private void GetPlayerOutOfCover(GameObject closestCover)
    {
        if (waitTime <= 0)
        {
            if (player.behindCover && Vector2.Distance(transform.position, closestCover.transform.position) < 3f)
            {
                player.behindCover = false;
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
            }
            waitTime = startWaitTimeCover;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}

