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
    private bool stop;

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
                if (alarmed)
                {
                    //enemy.Move();
                }
                else
                {
                    enemy.GetComponent<Animator>().SetBool("isPatroling", true);
                }
            } 

            if (enemy.GetComponent<Enemy>().detectionValue >= investigatingValue && enemy.GetComponent<Enemy>().detectionValue < alarmedValue)
            {
                if (alarmed)
                {
                    //enemy.Move();
                }
                else
                {
                    //StartCoroutine(enemy.GetComponent<Enemy>().WaitBeforeInvestigating());
                    enemy.GetComponent<Animator>().SetBool("isInvestigating1", true);


                    
                }
            }

            if(enemy.GetComponent<Enemy>().detectionValue >= alarmedValue && enemy.GetComponent<Enemy>().detectionValue < chasingValue)
            {
                //enemy.Move();
                alarmed = true;
            }

            if (enemy.GetComponent<Enemy>().detectionValue >= chasingValue)
            {
                //enemy.Chasing();
            }
            
        }
    }
}
