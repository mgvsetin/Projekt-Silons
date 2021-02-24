using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Variables

    public GameObject[] enemies;
    [SerializeField] private float investigatingValue;
    public float alarmedValue;
    [SerializeField] private float chasingValue;
    public bool alarmed;
    public bool chasing;

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {
        foreach(GameObject enemy in enemies)
        {
            if(enemy.GetComponent<Enemy>().detectionValue < investigatingValue)
           {
                if (alarmed && !chasing)
                {
                    enemy.GetComponent<Animator>().SetBool("isAlarmed", true);
                }
                else if (chasing)
                {
                    enemy.GetComponent<Animator>().SetBool("isChasing", true);
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
                if (alarmed && !chasing)
                {
                    enemy.GetComponent<Animator>().SetBool("isAlarmed", true);
                }
                else if (chasing)
                {
                    enemy.GetComponent<Animator>().SetBool("isChasing", true);
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
                if (chasing)
                {
                    enemy.GetComponent<Animator>().SetBool("isChasing", true);
                    enemy.GetComponent<Animator>().SetBool("isInvestigating1", false);
                    enemy.GetComponent<Animator>().SetBool("isInvestigating2", false);
                    enemy.GetComponent<Animator>().SetBool("isAlarmed", false);
                }
                else
                {
                    enemy.GetComponent<Animator>().SetBool("isAlarmed", true);
                    alarmed = true;
                }

            }

            if (enemy.GetComponent<Enemy>().detectionValue >= chasingValue)
            {
                enemy.GetComponent<Animator>().SetBool("isChasing", true);
                enemy.GetComponent<Animator>().SetBool("isInvestigating1", false);
                enemy.GetComponent<Animator>().SetBool("isInvestigating2", false);
                enemy.GetComponent<Animator>().SetBool("isAlarmed", false);
                chasing = true;
            }
            
        }
    }
}
