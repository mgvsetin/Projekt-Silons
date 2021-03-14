using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Variables

    public GameObject[] enemies;
    [SerializeField] private float investigatingValue;
    public float alarmedValue;
    public float chasingValue;
    public bool alarmed;
    public bool chasing;
    public Enemy enemyScript;

    void Start()
    {

    }

    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            enemyScript = enemy.GetComponent<Enemy>();

            if(enemyScript.detectionValue < investigatingValue)
           {
                if (enemyScript.alarmed && !enemyScript.chasing)
                {
                    enemy.GetComponent<Animator>().SetBool("isAlarmed", true);
                }
                else if (enemyScript.chasing)
                {
                    enemy.GetComponent<Animator>().SetBool("isChasing", true);
                    enemy.GetComponent<Animator>().SetBool("isPatroling", false);
                    enemy.GetComponent<Animator>().SetBool("isInvestigating1", false);
                    enemy.GetComponent<Animator>().SetBool("isInvestigating2", false);
                    enemy.GetComponent<Animator>().SetBool("isAlarmed", false);
                }
                else
                {
                    enemy.GetComponent<Animator>().SetBool("isPatroling", true);
                }
            } 

            if (enemy.GetComponent<Enemy>().detectionValue >= investigatingValue && enemy.GetComponent<Enemy>().detectionValue < alarmedValue)
            {
                if (enemyScript.alarmed && !enemyScript.chasing)
                {
                    enemy.GetComponent<Animator>().SetBool("isAlarmed", true);
                }
                else if (enemyScript.chasing)
                {
                    enemy.GetComponent<Animator>().SetBool("isChasing", true);
                    enemy.GetComponent<Animator>().SetBool("isPatroling", false);
                    enemy.GetComponent<Animator>().SetBool("isInvestigating1", false);
                    enemy.GetComponent<Animator>().SetBool("isInvestigating2", false);
                    enemy.GetComponent<Animator>().SetBool("isAlarmed", false);
                }
                else
                {
                    
                    enemy.GetComponent<Animator>().SetBool("isInvestigating1", true);
                }
            }

            if(enemy.GetComponent<Enemy>().detectionValue >= alarmedValue && enemy.GetComponent<Enemy>().detectionValue < chasingValue)
            {
                if (enemyScript.chasing)
                {
                    enemy.GetComponent<Animator>().SetBool("isChasing", true);
                    enemy.GetComponent<Animator>().SetBool("isPatroling", false);
                    enemy.GetComponent<Animator>().SetBool("isInvestigating1", false);
                    enemy.GetComponent<Animator>().SetBool("isInvestigating2", false);
                    enemy.GetComponent<Animator>().SetBool("isAlarmed", false);
                }
                else
                {
                    enemy.GetComponent<Animator>().SetBool("isAlarmed", true);
                    enemyScript.alarmed = true;
                }

            }

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
