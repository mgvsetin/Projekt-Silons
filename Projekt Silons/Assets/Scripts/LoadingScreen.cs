using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private RoomTemplates rooms;
    private float waitTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rooms = FindObjectOfType<RoomTemplates>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rooms.exitSpawned)
        {
            transform.GetComponent<Animator>().SetBool("Loaded", true);
            if(waitTime <= 0)
            {
                transform.parent.gameObject.SetActive(false);
            }
            else
            {
                waitTime -= Time.deltaTime;
            }

        }
    }
}
