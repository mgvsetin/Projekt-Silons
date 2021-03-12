using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyStopBeforeInvastigating : StateMachineBehaviour
{
    //Variables
    AIDestinationSetter aiDestinationSetter;
    AIPath aiPath;
    FieldOfView fov;
    Enemy enemy;
    float waitTime;
    float startWaitTime = 1f;
    public ParticleSystem questionMarkParticle;
    public AudioManager audioManager;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        aiDestinationSetter = animator.GetComponent<AIDestinationSetter>();
        waitTime = startWaitTime;
        fov = animator.GetComponentInChildren<FieldOfView>();
        aiPath = animator.GetComponent<AIPath>();
        Instantiate(questionMarkParticle, enemy.transform.position, Quaternion.identity);
        audioManager = FindObjectOfType<AudioManager>();

        audioManager.EnemySoundPlay("What was that");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(waitTime <= 0)
        {
            animator.SetBool("continueInvestaigating", true);
            aiPath.maxSpeed = enemy.moveSpeed;
        }
        else
        {
            aiPath.maxSpeed = 0;
            waitTime -= Time.deltaTime;
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
