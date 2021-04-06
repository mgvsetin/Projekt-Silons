using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Loading new level, or reseting this one, depends how you look at it
        if(collider.tag == "Player")
        {
            ScoreManager.instace.AddPoints();
            SceneManager.LoadScene(1);
        }
    }
}
