using JetBrains.Annotations;
using Pathfinding;
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
    private Transform target;
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
    [SerializeField] private bool onLastSeenPos;
    [SerializeField] private GameObject closestCover;
    [SerializeField] private float coverRayDistance;
    private GameObject tempClosestCover;

    public AIDestinationSetter aiDestinationSetter;
    private AIPath aiPath;


    private void Awake()
    {
        randomWaypoint = UnityEngine.Random.Range(0, waypoints.Length);
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        enemyManager = FindObjectOfType<EnemyManager>();
        fov = gameObject.GetComponentInChildren<FieldOfView>();
        aiDestinationSetter = gameObject.GetComponent<AIDestinationSetter>();
        aiPath = gameObject.GetComponent<AIPath>();
    }

    private void Update()
    {
        DetectingPlayer();
    }


    private void DetectingPlayer()
    {
        if (playerVisible)
        {
            //detectionBar.ShowDetectionBar();
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
        else
        {
            //detectionBar.HideDetectionBar();
        }

        detectionBar.SetDetectionValue(detectionValue);

    }

    public void InvestigatingParTwo()
    {
        if (aiPath.remainingDistance <= 0.5f)
        {
            Debug.Log("ABC");
            GameObject[] covers = GameObject.FindGameObjectsWithTag("Cover");
            SortCovers(covers);
            closestCover = null;

            for (int i = 0; i < covers.Length; i++)
            {
                if (closestCover == null)
                {
                    closestCover = covers[i];
                }
            }
            if (closestCover != null)
            {
                aiDestinationSetter.target = closestCover.transform;
                Debug.Log(aiDestinationSetter.target.name);
                if (aiPath.reachedEndOfPath)
                {
                    closestCover = null;
                }
            }
        }
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

    public void DecreaseDetectionValue()
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player Particle"))
        {
            Debug.Log("Particle Hit");
        }
        
    }
}

