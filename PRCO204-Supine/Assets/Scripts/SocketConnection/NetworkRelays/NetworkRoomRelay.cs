using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using SocketConnection.NetworkRelays;

public class NetworkRoomRelay : NetworkRelay
{
    
    /// <summary>
    /// Add this to something with a Door.cs script on it :)
    /// </summary>
    public class NetworkRoomData : NetworkData {
        // need to be public for conversion
        public string identifier;
        public string className;
        public float[] color;
        public float[] position;
        public Array doors;
        
        public NetworkRoomData(string identifier, string className, Color color, Transform transform, Array doors) {
            this.identifier = identifier;
            this.className = className;
            this.color = new []{ color.r, color.g, color.b };
            this.position = new[] {transform.position.x, transform.position.z};
            this.doors = doors;
        }
    }

    private Room room;
    
    [Header("For doors and their specific options")]
    public string identifier;
    public string className = "room";

    private string generateIdentifier() {
        return $"{className}-{Guid.NewGuid()}";
    }
    public new NetworkRoomData RelayData => new NetworkRoomData(identifier, className, room.GetColor(), room.transform, room.GetDoors());



    private void Awake() {
        room = gameObject.GetComponent<Room>();
        if (!room) {
            throw new Exception("This NetworkDoorRelay.cs file was attached to a gameobject without a Room script.");
        }
        if (string.IsNullOrEmpty(identifier)) {
            identifier = generateIdentifier();
        }
    }


    public void EnteredRoom() {
        ServerManager.Instance.EnteredRoom(RelayData);   
    }
    
    
}