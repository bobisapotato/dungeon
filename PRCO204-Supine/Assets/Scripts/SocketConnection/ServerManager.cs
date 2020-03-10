using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviour
{

    public GameObject _room;

    public class Item { }
    public class Bomb : Item { }

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
        Debug.Log(room.GetComponent<Collider>().bounds.size);

        x = recievedX;
        z = recievedZ;

        Debug.Log("Pos: " + x + ", 10, " + z + " Item: " + item);
    }

    void Start()
    {
        //DecodeMessage(0.5f, 0.5f, "bomb");
    }

}
