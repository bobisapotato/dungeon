using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public GameObject _room;
    public GameObject player;

    private WSConnection connection;
    public string server = "research.supine.dev:3018";

    public abstract class Item { public readonly GameObject Prefab; }

    private NetworkPositionRelay[] NetworkPositionRelays;

    [System.Serializable]
    public class Bomb : Item {
        public new GameObject Prefab;
    }
    public Bomb bomb;

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

        // DecodeMessage(0.5f, 0.5f, "bomb");
        // maths 0..1 -> -5..5

        Debug.Log("Spawning item");

        Vector3 roomSize = room.GetComponent<Collider>().bounds.size;
        Vector3 roomScale = room.GetComponent<Collider>().bounds.size / 2;

        Vector3 newPos = new Vector3((recievedX * roomSize.x) - roomScale.x, 0, (recievedZ * roomSize.z) - roomScale.z);

        //Debug.Log("Pos: " + x + ", 10, " + z + " Item: " + item);

        Instantiate(item.Prefab, newPos, Quaternion.identity /*, room.transform*/);
    }

    void Start() 
    {
        SetupRelays();
        // Initiate websocket connection here
        connection = gameObject.AddComponent<WSConnection>();
        connection.server = server;
        connection.OnSocketConnected += SocketStart;
        connection.OnNetworkTickRequest += NetworkTick;

        connection.OnSocketDisconnected += reason => Debug.Log("Disconnected from remote server. Reason: " + reason);

        //connection.OnSocketConnected += 
    }

    void SetupRelays() {
        NetworkPositionRelays = FindObjectsOfType<NetworkPositionRelay>();
    }

    void TriggerRelays() {
        foreach (var networkPositionRelay in NetworkPositionRelays) {
            connection.SendMessage("object:position", networkPositionRelay.RelayData);
        }
    }

    void SocketStart() {
        Debug.Log("Socket is started + event connection");
        connection.SendMessage("rooms:create");
    }

    void NetworkTick() {
        TriggerRelays();
    }

}
