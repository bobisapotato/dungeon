using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using SocketConnection.NetworkRelays;

public class NetworkPositionRelay : MonoBehaviour
    {
        public class NetworkPositionData : NetworkRelay.NetworkData {
            public float[] position;
            public string identifier;
            public NetworkPositionData(string identifier, Vector3 position) {
                this.position = new float[] { position.x, position.z};
                this.identifier = identifier;
                Debug.Log("NetworkPositionRelay: " + this.identifier);
                Debug.Log(this.position);
                Debug.Log(JsonConvert.SerializeObject(this));
            }
        }
        
        public string identifier;

        public NetworkPositionData RelayData {
            get { return new NetworkPositionData(identifier, gameObject.transform.position); }
        }
    }
