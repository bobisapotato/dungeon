using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

public class NetworkPositionRelay : MonoBehaviour
    {
        public class NetworkPositionData {
            private float[] position;
            private string identifier;
            public NetworkPositionData(string identifier, Vector3 position) {
                this.position = new float[] { position.x, position.z};
                this.identifier = identifier;
            }
        }
        
        public string identifier;

        public NetworkPositionData RelayData {
            get { return new NetworkPositionData(identifier, gameObject.transform.position); }
        }
    }
