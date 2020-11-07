using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRoom;

    public List<GameObject> rooms;

    public float waitTime;
    private bool exitSpawned;
    public GameObject exit;

    private void Update()
    {
        if(waitTime <= 0 && !exitSpawned)
        {
            Instantiate(exit, rooms[rooms.Count - 1].transform.position, Quaternion.identity);
            exitSpawned = true;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
