using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInvestigatingPartOne : StateMachineBehaviour
{
    //Variables
    Player player;
    Enemy enemy;
    bool ignoreCollider;
    AIDestinationSetter aiDestinationSetter;
    FieldOfView fov;
    AIPath aiPath;
    float waitTime;
    public float startWaitTime = 2f;
    public float moveSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = FindObjectOfType<Player>();

        aiDestinationSetter = animator.GetComponent<AIDestinationSetter>();
        fov =  animator.GetComponentInChildren<FieldOfView>();
        aiPath = animator.GetComponent<AIPath>();
        waitTime = startWaitTime;
        enemy = animator.GetComponent<Enemy>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiPath.maxSpeed = moveSpeed;
        if (enemy.heardSound == true)
        {
            aiDestinationSetter.target = enemy.lastHeardPosWaypoits[enemy.lastHeardPosWaypoits.Count - 1].transform;
        }
        else
        {
            aiDestinationSetter.target = fov.lastSeenPosWaypoits[fov.lastSeenPosWaypoits.Count - 1].transform;
        }

        if (Vector2.Distance(animator.transform.position, aiDestinationSetter.target.position) <= 1f)
        {
            animator.SetBool("isInvestigating2", true);
        }
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
