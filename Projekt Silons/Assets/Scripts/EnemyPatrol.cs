using Pathfinding;
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
    GameObject[] roomWaypoints;

    public AudioManager audioManager;


    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        waypoints = FindObjectOfType<WaypointTemplates>();
        string roomName = animator.transform.parent.name;

        switch (roomName)
        {
            case "B Room":
                roomWaypoints = waypoints.waypointsBRoom;
                break;

            case "L Room":
                roomWaypoints = waypoints.waypointsLRoom;
                break;
        }


        randomWaypoint = UnityEngine.Random.Range(0, roomWaypoints.Length);
        player = FindObjectOfType<Player>();
        aiDestinationSetter = animator.GetComponent<AIDestinationSetter>();
        audioManager = FindObjectOfType<AudioManager>();
        
        audioManager.EnemySoundPlay("Footsteps");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

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
}
