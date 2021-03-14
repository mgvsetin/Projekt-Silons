﻿using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : StateMachineBehaviour
{
    //Variables
     WaypointTemplates waypoints;
     int randomWaypoint;
     Player player;
     AIDestinationSetter aiDestinationSetter;
     bool ignoreCollider;
     float waitTime;
     float startWaitTime = 0.2f;
    Transform[] roomWaypoints;
    string roomName;
    private Enemy enemy;

    public AudioManager audioManager;
    private AudioSource audioToPlay;


    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        waypoints = animator.gameObject.GetComponent<WaypointTemplates>();
        roomName = animator.transform.parent.name;

        //SortWaypoints();
        roomWaypoints = waypoints.roomWaypoints;
        

        randomWaypoint = UnityEngine.Random.Range(0, roomWaypoints.Length);
        player = FindObjectOfType<Player>();
        aiDestinationSetter = animator.GetComponent<AIDestinationSetter>();
        audioManager = FindObjectOfType<AudioManager>();
        enemy = animator.GetComponent<Enemy>();

        audioToPlay = enemy.enemyAudioSources[0];
        audioToPlay.Play();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //SortWaypoints();
        SetTarget();

        if (Vector3.Distance(animator.transform.position, roomWaypoints[randomWaypoint].transform.position) < 0.80f)
        {
            if (waitTime <= 0)
            {
                randomWaypoint = UnityEngine.Random.Range(0, roomWaypoints.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    public void SetTarget()
    {
        aiDestinationSetter.target = roomWaypoints[randomWaypoint].transform;
    }

 /*   public void SortWaypoints()
    {

        switch (roomName)
        {
            case "B Room(Clone)":
                roomWaypoints = waypoints.waypointsBRoom;
                break;

            case "L Room(Clone)":
                roomWaypoints = waypoints.waypointsLRoom;
                break;

            case "R Room(Clone)":
                roomWaypoints = waypoints.waypointsRRoom;
                break;

            case "RB Room(Clone)":
                roomWaypoints = waypoints.waypointsRBRoom;
                break;

            case "RL Room(Clone)":
                roomWaypoints = waypoints.waypointsRLRoom;
                break;

            case "T Room(Clone)":
                roomWaypoints = waypoints.waypointsTRoom;
                break;

            case "TB Room(Clone)":
                roomWaypoints = waypoints.waypointsTBRoom;
                break;

            case "TL Room(Clone)":
                roomWaypoints = waypoints.waypointsTLRoom;
                break;

            case "TR Room(Clone)":
                roomWaypoints = waypoints.waypointsTRRoom;
                break;
        } */
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
   // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   // {

   // }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

