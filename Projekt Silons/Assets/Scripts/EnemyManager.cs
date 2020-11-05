using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //Variables

    public Enemy[] enemies;
    [SerializeField] private float investigatingValue;
    public float alarmedValue;
    [SerializeField] private float chasingValue;
    public bool alarmed;
    private bool stop;

    void Start()
    {
        enemies = FindObjectsOfType<Enemy>();
    }

    void Update()
    {
        foreach(Enemy enemy in enemies)
        {
            if(enemy.detectionValue < investigatingValue)
            {
                if (alarmed)
                {
                    //enemy.Move();
                }
                else
                {
                    enemy.Move();
                }
            }

            if (enemy.detectionValue >= investigatingValue && enemy.detectionValue < alarmedValue)
            {
                if (alarmed)
                {
                    //enemy.Move();
                }
                else
                {
                    StartCoroutine(enemy.WaitBeforeInvestigating());
                }
            }

            if(enemy.detectionValue >= alarmedValue && enemy.detectionValue < chasingValue)
            {
                //enemy.Move();
                alarmed = true;
            }

            if (enemy.detectionValue >= chasingValue)
            {
                //enemy.Chasing();
            }
            
        }
    }
}
