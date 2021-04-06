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
    private AudioSource audioToPlay;
    private Enemy enemy;
    private bool coinsRemoved = false;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiDestinationSetter = animator.GetComponent<AIDestinationSetter>();
        aiPath = animator.GetComponent<AIPath>();
        enemy = animator.GetComponent<Enemy>();
        animator.GetComponent<Enemy>().heardSound = false;
        animator.SetBool("continueInvestaigating", false);

        //Playing Audio
        audioToPlay = enemy.enemyAudioSources[0];
        audioToPlay.Play();

        //Removing Score
        if (!coinsRemoved)
        {
            ScoreManager.instace.RemoveSuspiciousPoints();
            coinsRemoved = true;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            GameObject[] covers = GameObject.FindGameObjectsWithTag("Cover");
            SortCovers(covers);
            closestCover = null;

            //Assigning closest cover
            for (int i = 0; i < covers.Length; i++)
            {
                if (closestCover == null)
                {
                    closestCover = covers[i];
                }
            }

            //Setting destination for cover and cheching if player is not hiding there
            if (closestCover != null)
            {
                aiDestinationSetter.target = closestCover.transform;
                if (Vector2.Distance(animator.transform.position, aiDestinationSetter.target.position) <= 2f)
                {
                    enemy.GetPlayerOutOfCover(closestCover);
                    closestCover = null;
                    animator.SetBool("isInvestigating2", false);
                    animator.SetBool("isInvestigating1", false);
                }
            }

        //Decreasing detection value
        animator.GetComponent<Enemy>().DecreaseDetectionValue();


        //Using selection sort for sorting covers
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
