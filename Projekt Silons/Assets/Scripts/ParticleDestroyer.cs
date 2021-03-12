using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    private float waitTime;
    private float startWaitTime = 10000f;

    // Update is called once per frame
    void Update()
    {
        if(waitTime <= 0f)
        {
            Destroy(gameObject);
            waitTime = startWaitTime;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
