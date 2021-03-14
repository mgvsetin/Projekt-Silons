using JetBrains.Annotations;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variables

    public float moveSpeed;
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

    public bool heardSound;
    public Transform lastHeardPos;
    public GameObject lastHeardWaypointPrefab;
    private GameObject lastHeardPosWaypointClone;
    [HideInInspector] public List<GameObject> lastHeardPosWaypoits;

    public bool alarmed;
    public bool chasing;
    [HideInInspector] public Animator animator;

    private AudioManager audioManager;
    public EnemySounds[] enemySounds;
    public List<AudioSource> enemyAudioSources;

    public bool crossedRooms;
    public GameObject crossedRoom;


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
        animator = gameObject.GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
        enemySounds = audioManager.enemySounds;
    }

    private void Update()
    {
        DetectingPlayer();
    }


    private void DetectingPlayer()
    {
        if (playerVisible)
        {
            if (alarmed && !newDetectionValueSet && detectionValue < enemyManager.alarmedValue)
            {
                detectionValue = enemyManager.alarmedValue + 1f;
                newDetectionValueSet = true;
            }

            if (detectionValue < maxDetectionValue)
            {
                detectionValue += detectionSpeed * Time.deltaTime;
            }
        }

        if (heardSound)
        {
            if (alarmed && detectionValue < enemyManager.alarmedValue)
            {
                detectionValue = enemyManager.alarmedValue + 1f;
            }
        }

        detectionBar.SetDetectionValue(detectionValue);

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
            lastHeardPos = player.transform;
            lastHeardPosWaypointClone = Instantiate(lastHeardWaypointPrefab, lastHeardPos.position, Quaternion.identity) ;
            lastHeardPosWaypoits.Add(lastHeardPosWaypointClone);

            if(detectionValue < enemyManager.chasingValue)
            {
                detectionValue += 1.1f;
            }
            heardSound = true;
        }

        if (collider.CompareTag("Rooms"))
        {
            crossedRooms = true;
            crossedRoom = collider.gameObject;
        }
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<CircleCollider2D>());
        }
    }

    public void EnemySoundPlay(string name)
    {
        foreach(AudioSource audio in enemyAudioSources)
        {
            if(audio.clip.name == name)
            {
                if (!PauseMenu.isPaused)
                {
                    audio.Play();
                }
                else
                {
                    audio.Pause();
                }
            }
        }

       /* EnemySounds es = Array.Find(enemySounds, enemysound => enemysound.name == name);
        if (!PauseMenu.isPaused)
        {
            es.source.Play();
        }
        else
        {
            es.source.Pause();
        } */
    }

    public void StopEnemySound(string name)
    {
        EnemySounds es = Array.Find(enemySounds, enemysound => enemysound.name == name);
        es.source.Stop();
    }
}

