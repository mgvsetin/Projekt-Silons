using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInvestigatingPartTwo : StateMachineBehaviour
{
    //Variables
    GameObject closestCover;
    AIDestinationSetter aiDestinationSetter;
    AIPath aiPath;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiDestinationSetter = animator.GetComponent<AIDestinationSetter>();
        aiPath = animator.GetComponent<AIPath>();
        animator.GetComponent<Enemy>().heardSound = false;
        animator.SetBool("continueInvestaigating", false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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
                if (Vector2.Distance(animator.transform.position, aiDestinationSetter.target.position) <= 1.5f)
                {
                    closestCover = null;
                    animator.SetBool("isInvestigating2", false);
                    animator.SetBool("isInvestigating1", false);
                }
            }

        animator.GetComponent<Enemy>().DecreaseDetectionValue();

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
