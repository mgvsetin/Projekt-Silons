using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag != "Player" || collider.tag != "Player Particle")
        {
            Destroy(collider.gameObject);
        }
    }
}
