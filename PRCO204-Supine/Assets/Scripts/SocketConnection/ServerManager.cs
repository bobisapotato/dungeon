using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{

    public GameObject _room;

    public GameObject BombPrefab;

    public class Item { }
    public class Bomb : Item { }

    public class SpawnQueueItem {
        float x;
        float z;
        Item item;
        GameObject room;

        public SpawnQueueItem(float recievedX, float recievedZ, Item item, GameObject room) {
            this.x = recievedX;
            this.z = recievedZ;
            this.item = item;
            this.room = room;
        }
    }

    public void DecodeMessage(float recievedX, float recievedZ, string itemName)
    {
        Debug.Log("Spawning " + itemName);
        GameObject room = _room;

        if (itemName == "bomb")
        {
            SpawnItem(recievedX, recievedZ, new Bomb(), room);
        }
    }



    public void SpawnItem(float recievedX, float recievedZ, Item item, GameObject room) 
    {
        float x, z;

        // maths 0..1 -> -5..5

        Debug.Log("Spawning item");

        Vector3 roomSize = room.GetComponent<Collider>().bounds.size;
        Vector3 roomScale = room.GetComponent<Collider>().bounds.size / 2;

        Vector3 newPos = new Vector3((recievedX * roomSize.x) - roomScale.x, 0, (recievedZ * roomSize.z) - roomScale.z);

        //Debug.Log("Pos: " + x + ", 10, " + z + " Item: " + item);

        Instantiate(BombPrefab, newPos, Quaternion.identity /*, room.transform*/);
    }

    void Start()
    {
        //DecodeMessage(0.5f, 0.5f, "bomb");
    }

    void Update() {
    }

}
