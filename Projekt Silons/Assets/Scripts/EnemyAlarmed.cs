using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAlarmed : StateMachineBehaviour
{
    //Variables
    GameObject closestCover;
    int randomCover;
    float waitTime;
    float startWaitTime = 0.2f;
    AIDestinationSetter aiDestinationSetter;
    GameObject[] covers;
    Enemy enemy;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        aiDestinationSetter = animator.GetComponent<AIDestinationSetter>();
        covers = GameObject.FindGameObjectsWithTag("Cover");
        randomCover = UnityEngine.Random.Range(0, covers.Length);
        waitTime = startWaitTime;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        covers = GameObject.FindGameObjectsWithTag("Cover");

        if (enemy.heardSound)
        {
            animator.SetBool("isInvestigating1", true);
        }
        else
        {
            SetTarget();
        }
       

        for (int i = 0; i < covers.Length; i++)
        {

            if (Vector3.Distance(animator.transform.position, covers[randomCover].transform.position) < 2f)
            {
                if (waitTime <= 0)
                {
                    randomCover = UnityEngine.Random.Range(0, covers.Length);
                    waitTime = startWaitTime;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }


        GameObject[] SortCovers(GameObject[] unsortedCovers)
        {
            int min;
            GameObject temp;

            for (int i = 0; i < unsortedCovers.Length; i++)
            {
                min = i;
                for (int j = i + 1; j < unsortedCovers.Length; j++)
                {
                    if (Vector2.Distance(animator.transform.position, unsortedCovers[j].transform.position) < Vector2.Distance(animator.transform.position, unsortedCovers[min].transform.position))
                    {
                        min = j;
                    }
                }

                if (min != i)
                {
                    temp = unsortedCovers[i];
                    unsortedCovers[i] = unsortedCovers[min];
                    unsortedCovers[min] = temp;
                }
            }
            return unsortedCovers;
        }
    }

    public void SetTarget()
    {
        aiDestinationSetter.target = covers[randomCover].transform;
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
