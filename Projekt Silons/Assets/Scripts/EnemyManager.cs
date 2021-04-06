using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Variables

    public float chasingRadius;
    public GameObject[] enemies;
    [SerializeField] private float investigatingValue;
    public float alarmedValue;
    public float chasingValue;
    public bool alarmed;
    public bool chasing;
    public Enemy enemyScript;

    void Update()
    {

        //*If enemy was in alarmed state it never goes to patroling state. If detection value is that of patroling state I still set the state to alarmed. And if enemy is chasing player it doesnt matter which detection value it has it will always be chasing player until he loses him, then he comes to alarmed state

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            enemyScript = enemy.GetComponent<Enemy>();

            //Patroloing state
            if(enemyScript.detectionValue < investigatingValue)
           {
                //Checking if was alarmed
                if (enemyScript.alarmed && !enemyScript.chasing)
                {
                    enemy.GetComponent<Animator>().SetBool("isAlarmed", true);
                }
                //Checking if chasing player
                else if (enemyScript.chasing)
                {
                    enemy.GetComponent<Animator>().SetBool("isChasing", true);
                    enemy.GetComponent<Animator>().SetBool("isPatroling", false);
                    enemy.GetComponent<Animator>().SetBool("isInvestigating1", false);
                    enemy.GetComponent<Animator>().SetBool("isInvestigating2", false);
                    enemy.GetComponent<Animator>().SetBool("isAlarmed", false);
                }
                //Setting patroling state
                else
                {
                    enemy.GetComponent<Animator>().SetBool("isPatroling", true);
                }
            } 

            if (enemy.GetComponent<Enemy>().detectionValue >= investigatingValue && enemy.GetComponent<Enemy>().detectionValue < alarmedValue)
            {
                //Checking if was alarmed
                if (enemyScript.alarmed && !enemyScript.chasing)
                {
                    enemy.GetComponent<Animator>().SetBool("isAlarmed", true);
                }
                //Checking if chasing player
                else if (enemyScript.chasing)
                {
                    enemy.GetComponent<Animator>().SetBool("isChasing", true);
                    enemy.GetComponent<Animator>().SetBool("isPatroling", false);
                    enemy.GetComponent<Animator>().SetBool("isInvestigating1", false);
                    enemy.GetComponent<Animator>().SetBool("isInvestigating2", false);
                    enemy.GetComponent<Animator>().SetBool("isAlarmed", false);
                }
                //Setting investigating state
                else
                {
                    
                    enemy.GetComponent<Animator>().SetBool("isInvestigating1", true);
                }
            }

            if(enemy.GetComponent<Enemy>().detectionValue >= alarmedValue && enemy.GetComponent<Enemy>().detectionValue < chasingValue)
            {
                //Checking if chasing player
                if (enemyScript.chasing)
                {
                    enemy.GetComponent<Animator>().SetBool("isChasing", true);
                    enemy.GetComponent<Animator>().SetBool("isPatroling", false);
                    enemy.GetComponent<Animator>().SetBool("isInvestigating1", false);
                    enemy.GetComponent<Animator>().SetBool("isInvestigating2", false);
                    enemy.GetComponent<Animator>().SetBool("isAlarmed", false);
                }
                //Setting alarmed state
                else
                {
                    enemy.GetComponent<Animator>().SetBool("isAlarmed", true);
                    enemyScript.alarmed = true;
                }

            }

            //Setting chasing state
            if (enemy.GetComponent<Enemy>().detectionValue >= chasingValue)
            {
                enemy.GetComponent<Animator>().SetBool("isChasing", true);
                enemy.GetComponent<Animator>().SetBool("isPatroling", false);
                enemy.GetComponent<Animator>().SetBool("isInvestigating1", false);
                enemy.GetComponent<Animator>().SetBool("isInvestigating1", false);
                enemy.GetComponent<Animator>().SetBool("isInvestigating2", false);
                enemy.GetComponent<Animator>().SetBool("isAlarmed", false);
                enemyScript.chasing = true;
            }
            
        }
    }
}
