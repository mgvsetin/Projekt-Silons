using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyChasing : StateMachineBehaviour
{
    //Variables
    public float chasingRadius;
    public AIDestinationSetter aiDestinationSetter;
    GameObject player;
    EnemyManager enemyManager;
    Enemy enemy;
    AIPath aIPath;
    AudioManager audioManager;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        animator.SetBool("isAlarmed", false);
        aiDestinationSetter = animator.GetComponent<AIDestinationSetter>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyManager = FindObjectOfType<EnemyManager>();
        aIPath = animator.GetComponent<AIPath>();
        audioManager = FindObjectOfType<AudioManager>();

        enemy.StopEnemySound("What was that");
        enemy.StopEnemySound("Somewhere");
        enemy.EnemySoundPlay("He's here");

        aiDestinationSetter.target = player.transform;
        aIPath.maxSpeed = enemy.moveSpeed;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiDestinationSetter.target = player.transform;

        if (Vector2.Distance(animator.transform.position, player.transform.position) >= chasingRadius)
         {
            enemy.heardSound = false;

            enemy.DecreaseDetectionValue();
            if(enemy.detectionValue <= 0f)
            {
                enemy.chasing = false;
                animator.SetBool("isChasing", false);
            }
         }
        else
        {
            enemy.detectionValue = enemyManager.chasingValue + 2f;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
