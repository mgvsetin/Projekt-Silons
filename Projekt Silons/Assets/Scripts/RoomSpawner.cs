using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    /* opening direction    = 1 = bottom door   = connects to top door
                            = 2 = top door      = connects to bottom door
                            = 3 = left door     = connects to right door
                            = 4 = right door    = connects to left door
    */
    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;
    public GameObject destroyer;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        destroyer = GameObject.FindGameObjectWithTag("Destroyer");
        Invoke("Spawn", 0.1f);
        Invoke("PostSpawnOperations", 0.2f);
    }

    private void Spawn()
    {
        if (!spawned)
        {
            switch (openingDirection)
            {
                case 1:
                    //spawns bottom door
                    rand = Random.Range(0, templates.bottomRooms.Length);
                    Instantiate(templates.bottomRooms[rand], transform.position, Quaternion.identity);
                    break;

                case 2:
                    //spawns top door
                    rand = Random.Range(0, templates.topRooms.Length);
                    Instantiate(templates.topRooms[rand], transform.position, Quaternion.identity);
                    break;

                case 3:
                    //spawns left door
                    rand = Random.Range(0, templates.leftRooms.Length);
                    Instantiate(templates.leftRooms[rand], transform.position, Quaternion.identity);
                    break;

                case 4:
                    //spawns right door
                    rand = Random.Range(0, templates.rightRooms.Length);
                    Instantiate(templates.rightRooms[rand], transform.position, Quaternion.identity);
                    break;
            }
            spawned = true;
        }
    }

    private void PostSpawnOperations()
    {
        Destroy(destroyer);
        AstarPath.active.Scan();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Room Spawn Point"))
        {
            //Fixing opening bugs
            if(collider.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                //spawn walls blocking openings
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
