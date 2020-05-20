using System.Data.Common;
using SocketConnection.NetworkRelays;
using UnityEngine;

public class NetworkDataRelay : NetworkRelay {
        [Header("For objects with abstract data")]
        public string identifier;
        public string attribute;
        public object value;
            
        public class NetworkDataData : NetworkData {
            private string identifier;
            private string attribute;
            private object value;

            public NetworkDataData(string identifier, string attribute, object value) {
                this.identifier = identifier;
                this.attribute = attribute;
                this.value = value;
            }
        }

        public NetworkDataData RelayData {
            get => new NetworkDataData(identifier, attribute, value);
        }

        public void SetData(object value) {
            this.value = value;
        }
        
    }