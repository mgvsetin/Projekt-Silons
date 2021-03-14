using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSort : MonoBehaviour
{
    public string roomName;
    public Transform[] waypoints;
    WaypointTemplates waypointTemplates;

    // Start is called before the first frame update
    void Start()
    {
        waypointTemplates = FindObjectOfType<WaypointTemplates>();
        roomName = transform.parent.name;
    }

    // Update is called once per frame
    void Update()
    {
        waypoints = transform.GetComponentsInChildren<Transform>();

      /*  switch (roomName)
        {
            case "B Room(Clone)":
                waypointTemplates.waypointsBRoom = waypoints;
                break;

            case "L Room(Clone)":
                waypointTemplates.waypointsLRoom = waypoints;
                break;

            case "R Room(Clone)":
                waypointTemplates.waypointsRRoom = waypoints;
                break;

            case "RB Room(Clone)":
                waypointTemplates.waypointsRBRoom = waypoints;
                break;

            case "RL Room(Clone)":
                waypointTemplates.waypointsRLRoom = waypoints;
                break;

            case "T Room(Clone)":
                waypointTemplates.waypointsTRoom = waypoints;
                break;

            case "TB Room(Clone)":
                waypointTemplates.waypointsTBRoom = waypoints;
                break;

            case "TL Room(Clone)":
                waypointTemplates.waypointsTLRoom = waypoints;
                break;

            case "TR Room(Clone)":
                waypointTemplates.waypointsTRRoom = waypoints;
                break;
        } */
    }
}
