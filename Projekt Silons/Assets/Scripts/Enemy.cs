using JetBrains.Annotations;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    //Variables

    public float moveSpeed;
    public bool attacking;
    public Player player;
    public Transform[] waypoints;
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


    private void Start()
    {
        //Setting Variable values
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
        //Checking for player
        DetectingPlayer();
    }


    private void DetectingPlayer()
    {
        //Enemy sees player
        if (playerVisible)
        {
            //if alarmed starting point for adding detection is not 0, but alarmedValue
            if (alarmed && !newDetectionValueSet && detectionValue < enemyManager.alarmedValue)
            {
                detectionValue = enemyManager.alarmedValue + 1f;
                newDetectionValueSet = true;
            }

            //Add detection value
            if (detectionValue < maxDetectionValue)
            {
                detectionValue += detectionSpeed * Time.deltaTime;
            }
        }

        //Enemy heard player
        if (heardSound)
        {
            //Starting from alarmedValue if alarmed
            if (alarmed && detectionValue < enemyManager.alarmedValue)
            {
                detectionValue = enemyManager.alarmedValue + 1f;
            }
        }

        //Adding detection value
        detectionBar.SetDetectionValue(detectionValue);

    }

    public void DecreaseDetectionValue()
    {
        if (detectionValue > 0)
        {
            detectionValue -= detectionSpeed * Time.deltaTime;
        }
    }

    public void GetPlayerOutOfCover(GameObject cover)
    {
        if (player.behindCover)
        {
            //Checking if enemy came to the cover he went to
            if(Vector2.Distance(transform.position, cover.transform.position) < 2f)
            {
                //Checking if the cover enemy is at is the same as the cover player is in
                if (Vector2.Distance(player.coverImIn.position, cover.transform.position) < 2f)
                {
                    SceneManager.LoadScene(2);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player Particle"))
        {
            //Creating last heard position waypoint
            lastHeardPos = player.transform;
            lastHeardPosWaypointClone = Instantiate(lastHeardWaypointPrefab, lastHeardPos.position, Quaternion.identity) ;
            lastHeardPosWaypoits.Add(lastHeardPosWaypointClone);

            //Adding detection value
            if(detectionValue < enemyManager.chasingValue)
            {
                detectionValue += 1.1f;
            }
            heardSound = true;
        }

        //Checking if enemy went to another room
        if (collider.CompareTag("Rooms"))
        {
            crossedRooms = true;
            //Setting the room he went to as the room he stays in
            crossedRoom = collider.gameObject;
        }

        if (collider.CompareTag("Player"))
        {
            if(chasing)
            {   
                //Game over if chasing and touched player
                SceneManager.LoadScene(2);
            }
            else if(detectionValue < enemyManager.chasingValue + 1f)
            {
                //if not chasing and touched start chasing
                detectionValue = enemyManager.chasingValue + 1f;
            }
        }
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            //ignore collision with other enemies
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
    }

    public void StopEnemySound(string name)
    {
        EnemySounds es = Array.Find(enemySounds, enemysound => enemysound.name == name);
        es.source.Stop();
    }
}

