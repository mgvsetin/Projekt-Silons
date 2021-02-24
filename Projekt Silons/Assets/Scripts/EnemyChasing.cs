using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyChasing : StateMachineBehaviour
{
    //Variables
    public float chasingRadius;
    bool inRadius;
    public LayerMask whatIsPlayer;
    public AIDestinationSetter aiDestinationSetter;
    Player player;
    EnemyManager enemyManager;
    Enemy enemy;
    FieldOfView fov;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        fov = animator.GetComponentInChildren<FieldOfView>();
        enemy = animator.GetComponent<Enemy>();
        animator.SetBool("isAlarmed", false);
        aiDestinationSetter = animator.GetComponent<AIDestinationSetter>();
        player = FindObjectOfType<Player>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inRadius = Physics2D.OverlapCircle(animator.transform.position, chasingRadius, whatIsPlayer);

        if (Vector2.Distance(animator.transform.position, player.transform.position) <= chasingRadius)
        {
            aiDestinationSetter.target = player.transform;
            Debug.Log("In");
        }
        else
        {
            enemy.detectionValue = 0f;
            enemy.heardSound = false;

            enemyManager.chasing = false;
            animator.SetBool("isChasing", false);
            Debug.Log("Out");
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
