using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;

public class EnemyChasing : StateMachineBehaviour
{
    //Variables

    public AIDestinationSetter aiDestinationSetter;
    GameObject player;
    EnemyManager enemyManager;
    Enemy enemy;
    AIPath aIPath;
    AudioSource audioToPlay;
    GameObject[] enemies;
    private bool coinsRemoved = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        animator.SetBool("isAlarmed", false);
        aiDestinationSetter = animator.GetComponent<AIDestinationSetter>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyManager = FindObjectOfType<EnemyManager>();
        aIPath = animator.GetComponent<AIPath>();

        //Playing Audio
        audioToPlay = enemy.enemyAudioSources[0];
        audioToPlay.Play();
        audioToPlay = enemy.enemyAudioSources[3];
        audioToPlay.Play();

        //Unstoping
        aiDestinationSetter.target = player.transform;
        aIPath.maxSpeed = enemy.moveSpeed;

        //Removing Score
        if (!coinsRemoved)
        {
            ScoreManager.instace.RemoveAlarmedPoints();
            coinsRemoved = true;
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiDestinationSetter.target = player.transform;

        if (Vector2.Distance(animator.transform.position, player.transform.position) >= enemyManager.chasingRadius)
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
            if (player.GetComponent<Player>().behindCover)
            {
                if (Vector2.Distance(player.GetComponent<Player>().coverImIn.position, animator.transform.position) < 2f)
                {
                    SceneManager.LoadScene(2);
                }
            }
        }

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (Vector2.Distance(animator.transform.position, enemy.transform.position) <= FindObjectOfType<EnemyManager>().chasingRadius)
            {
                if(Vector2.Distance(enemy.transform.position, player.transform.position) <= enemyManager.chasingRadius)
                {
                    enemy.GetComponent<Enemy>().chasing = true;
                    enemy.GetComponent<Enemy>().detectionValue = enemyManager.chasingValue + 2f;
                }
                else
                {
                    enemy.GetComponent<Enemy>().heardSound = false;

                    enemy.GetComponent<Enemy>().DecreaseDetectionValue();
                    if (enemy.GetComponent<Enemy>().detectionValue <= 0f)
                    {
                        enemy.GetComponent<Enemy>().chasing = false;
                        animator.SetBool("isChasing", false);
                    }
                }

            }
            else
            {
                enemy.GetComponent<Enemy>().DecreaseDetectionValue();
            }
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
