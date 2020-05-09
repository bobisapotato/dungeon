using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using SocketConnection.NetworkRelays;

public class NetworkEntityRelay : NetworkRelay
{
    public class NetworkEntityData : NetworkData {
        // need to be public for conversion
        public float[] position;
        public float rotation;
        public string identifier;
        public string className;
        public NetworkEntityData(string identifier, string className, Vector3 position, Quaternion rotation) {
            this.position = new float[] { position.x, position.z};
            this.rotation = rotation.y;
            this.identifier = identifier;
            this.className = className;
        }
    }
        
    [Header("For objects with position, rotation, health, class")]
    public string identifier;
    public string className;

    private string generateIdentifier() {
        return $"{className}-{Guid.NewGuid()}";
    }
    
    

    public new NetworkEntityData RelayData => new NetworkEntityData(identifier, className, gameObject.transform.position, gameObject.transform.localRotation);

    
    /// <summary>
    /// ServerManager.Instance needs to wait for the connection to be set up.
    /// The ServerManager will get all available relays once it's set up so we don't need to do any queueing
    /// These functions will then execute if they're instantiated after the network was originally set up. 
    /// </summary>
    private void Start() {
        if (!ServerManager.Instance) return;
        ServerManager.Instance.CreateRelay(this);
        ServerManager.Instance.AddToRelaySet(this);
    }

    private void OnDestroy() {
        if (!ServerManager.Instance) return;
        ServerManager.Instance.RemoveFromRelaySet(this);
        ServerManager.Instance.DestroyRelay(this);
    }

    private void Awake() {
        if (string.IsNullOrEmpty(identifier)) {
            identifier = generateIdentifier();
        }
    }
}