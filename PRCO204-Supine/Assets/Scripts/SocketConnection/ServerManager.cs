using System;
using System.Collections;
using System.Collections.Generic;
using SocketConnection.NetworkRelays;
using UnityEditor.MemoryProfiler;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ServerManager : MonoBehaviour {
    
    // Singleton it
    private static ServerManager _mInstance;

    public static ServerManager Instance  {
        get {
            Debug.Log(_mInstance);
            if (_mInstance == null) {
                //_mInstance = 
            }
            Debug.Log(_mInstance);
            return _mInstance;
        }
    }
    
    public void Awake() {
        _mInstance = this;
    }



    private WSConnection connection;
    public string server = "research.supine.dev:3018";

    // Old item system broke, no idea when
    public GameObject itemBomb;
    public GameObject itemHealthPotion;


    private string _roomCode;
    public string RoomCode {
        get { return _roomCode; }
        set {
            _roomCode = value;
            UpdateRoomCodeUI(_roomCode);
        }
    }

    public void DecodeMessage(float recievedX, float recievedZ, string itemName) {
        Debug.Log("Spawning " + itemName);
        

        if (itemName == "bomb") {
            SpawnItem(recievedX, recievedZ, itemBomb, previousRoom.gameObject);
        }
        else if (itemName == "healthPotion")
        {
            SpawnItem(recievedX, recievedZ, itemHealthPotion, previousRoom.gameObject);
        }
    }


    public void SpawnItem(float recievedX, float recievedZ, GameObject prefab, GameObject room) {
        float x, z;
        float y = 10f;

        // DecodeMessage(0.5f, 0.5f, "bomb");
        // maths 0..1 -> -5..5

        Debug.Log("Spawning item");

        Vector3 bounds = new Vector3(15, 0, 15);
        Vector3 roomSize = bounds/*room.GetComponent<Collider>().bounds.size*/;
        Vector3 roomScale = bounds/*room.GetComponent<Collider>().bounds.size*/ / 2;

        Vector3 newPos = new Vector3((recievedX * roomSize.x) - roomScale.x, y,
            (recievedZ * roomSize.z) - roomScale.z);

        //Debug.Log("Pos: " + x + ", 10, " + z + " Item: " + item);

        Instantiate(prefab, newPos, Quaternion.identity /*, room.transform*/);
    }

    void Start() {
        _mInstance = this;
        SetupRelays();
        // Initiate websocket connection here
        connection = gameObject.AddComponent<WSConnection>();
        connection.server = server;
        connection.OnSocketConnected += SocketStart;
        connection.OnNetworkTickRequest += NetworkTick;

        connection.OnSocketError += reason => Debug.LogWarning("Socket error: " + reason);

        connection.OnSocketDisconnected +=
            reason => Debug.Log("Disconnected from remote server. Reason: " + reason);

        connection.Connect();

        //connection.OnSocketConnected += 
    }

    void SetupRelays() {
        NetworkPositionRelays = new List<NetworkPositionRelay>(FindObjectsOfType<NetworkPositionRelay>());
        NetworkEntityRelays = new List<NetworkEntityRelay>(FindObjectsOfType<NetworkEntityRelay>());

    }

    void TriggerRelays() {
        foreach (var relay in NetworkPositionRelays) {
            connection.SendMessage("object:position", relay.RelayData);
        }
        foreach (var relay in NetworkEntityRelays) {
            connection.SendMessage("object:position", relay.RelayData);
        }
    }

    // Pretty polymorphism

    private List<NetworkRelay> NetworkRelays = new List<NetworkRelay>();
    private List<NetworkDataRelay> NetworkDataRelays = new List<NetworkDataRelay>();
    private List<NetworkPositionRelay> NetworkPositionRelays = new List<NetworkPositionRelay>();
    private List<NetworkEntityRelay> NetworkEntityRelays = new List<NetworkEntityRelay>();
    public void AddToRelaySet(NetworkRelay relay) => NetworkRelays.Add(relay);
    public void AddToRelaySet(NetworkPositionRelay relay) => NetworkPositionRelays.Add(relay);
    public void AddToRelaySet(NetworkDataRelay relay) => NetworkDataRelays.Add(relay);
    public void AddToRelaySet(NetworkEntityRelay relay) => NetworkEntityRelays.Add(relay);
    
    public void RemoveFromRelaySet(NetworkRelay relay) => NetworkRelays.Remove(relay);
    public void RemoveFromRelaySet(NetworkPositionRelay relay) => NetworkPositionRelays.Remove(relay);
    public void RemoveFromRelaySet(NetworkDataRelay relay) => NetworkDataRelays.Remove(relay);
    public void RemoveFromRelaySet(NetworkEntityRelay relay) => NetworkEntityRelays.Remove(relay);

    // Run creation events (eg send spawn player event)
    public void CreateRelay(NetworkRelay relay) {}
    // Destroy events (eg destroy player event)
    public void DestroyRelay(NetworkRelay relay) { }

    public void CreateRelay(NetworkEntityRelay relay) => connection.SendMessage("entity:create", relay.RelayData);
    public void DestroyRelay(NetworkEntityRelay relay) => connection.SendMessage("entity:destroy", relay.RelayData);

    private Room previousRoom;
    private NetworkRoomRelay.NetworkRoomData previousRoomData;
    public void EnteredRoom(NetworkRoomRelay.NetworkRoomData data, Room room) {
        previousRoomData = data;
        previousRoom = room;
        connection.SendMessage("room:update", data);
    }


    void SocketStart() {
        Debug.Log("Socket is started + event connection");
        connection.SendMessage("rooms:create");
    }

    /// <summary>
    /// Call this when there's a new player 2 client that needs data backfilled.
    /// </summary>
    void ClientConnectionResponse() {
        Debug.Log($"Backfill: {NetworkEntityRelays.Count} NERs, {NetworkRelays.Count} NRs");
        NetworkRelays.ForEach(CreateRelay);
        NetworkEntityRelays.ForEach(CreateRelay);
        // Cache the last room update
        connection.SendMessage("room:update", previousRoomData);
    }

    void NetworkTick() {
        TriggerRelays();
    }

    void UpdateRoomCodeUI(string roomCode) {
        UIManager ui = FindObjectOfType<UIManager>();
        ui.SetText(roomCode);
    }

    public void DispatchMessage(string action, ArrayList data) {
        
        
        //Debug.Log("Dispatching message: " + action);
        if (action == "rooms:joined") {
            // Joined a room :D
            string roomCode = (string) data[0];
            RoomCode = roomCode;
            //string roomCode = data;
        }
        if (action == "rooms:backfill") {
            // a web client has connected successfully to this room
            ClientConnectionResponse();
        }

        if (action == "game:action") {
            // (arrlist shift)
            string actionType = (string) data[0];
            data.RemoveAt(0);
            DecodeMessage((float) ((double) data[0]), (float) ((double) data[1]), actionType);
        }
    }
    
    
}