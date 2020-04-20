using System;
using UnityEngine;

namespace SocketConnection.NetworkRelays {
    public abstract class NetworkRelay : MonoBehaviour {
        public abstract class NetworkData {
            public string identifier;
        }

        public NetworkData RelayData;


        private void Start() {
            Debug.Log("Relay awakened");
            Debug.Log(ServerManager.Instance);
            ServerManager.Instance.CreateRelay(this);
            ServerManager.Instance.AddToRelaySet(this);
        }

        private void OnDestroy() {
            ServerManager.Instance.RemoveFromRelaySet(this);
            ServerManager.Instance.DestroyRelay(this);
        }
    }
}