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
    public GameObject[] covers;
    Enemy enemy;
    string roomName;
    CoverTemplates coverTemplates;
    public GameObject[] enemies;
    public GameObject currentCover;

    public AudioSource audioToPlay;
    private bool coinsRemoved = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //Setting Variables
        enemy = animator.GetComponent<Enemy>();
        aiDestinationSetter = animator.GetComponent<AIDestinationSetter>();
        randomCover = UnityEngine.Random.Range(0, covers.Length);
        waitTime = startWaitTime;
        coverTemplates = animator.gameObject.GetComponent<CoverTemplates>();

        //Setting which covers are in room where enemy starts
        covers = coverTemplates.startingRoomCovers;

        //Playing Audio
        audioToPlay = enemy.enemyAudioSources[2];
        audioToPlay.Play();
        audioToPlay = enemy.enemyAudioSources[0];
        audioToPlay.Play();

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
        //Heard or saw player again, act like Investigating state, then come back to this state again
        if (enemy.heardSound)
        {
            animator.SetBool("isInvestigating1", true);
        }

        if (enemy.playerVisible)
        {
            animator.SetBool("isInvestigating1", true);
        }
        else
        {
            //Look in covers for player, if he's there game over
            SetTarget();
            enemy.GetPlayerOutOfCover(currentCover);
        }

        if (enemy.crossedRooms)
        {
            //Setting enemy to patrol in new room if he crossed rooms
            covers = enemy.crossedRoom.GetComponent<CoverTemplates>().currentRoomCovers;
        }

        //Making Enemies in radius alarmed as well

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            if(Vector2.Distance(animator.transform.position, enemy.transform.position) <= FindObjectOfType<EnemyManager>().chasingRadius)
            {
                enemy.GetComponent<Enemy>().alarmed = true;
            }
        } 
       
        //Choose random cover in room
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
        if(enemy.playerVisible == false)
        {
            animator.GetComponent<Enemy>().DecreaseDetectionValue();
        }

    }

    //Seting destination in A*
    public void SetTarget()
    {
        currentCover = covers[randomCover];
        aiDestinationSetter.target = currentCover.transform;

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
