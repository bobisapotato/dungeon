using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using WebSocketSharp;
using Newtonsoft.Json;
using UnityEditor.MemoryProfiler;

public class WSConnection : MonoBehaviour {
    public string server;
    private WebSocket socket;
    private Coroutine NetworkTickCoroutine;

    public ServerManager serverManager;

    #region Events

    public delegate void SocketConnected();

    public event SocketConnected OnSocketConnected;

    public delegate void NetworkTickRequest();

    public event NetworkTickRequest OnNetworkTickRequest;

    public delegate void SocketDisconnected(String reason);

    public event SocketDisconnected OnSocketDisconnected;

    public delegate void SocketError(String error);

    public event SocketError OnSocketError;

    #endregion


    abstract class NetworkEvent {
        public abstract void Dispatch();
    }

    class BasicNetworkEvent : NetworkEvent {
        private string action;
        private ArrayList data;
        private ServerManager serverManager;

        public BasicNetworkEvent(string action, ArrayList data, ServerManager serverManager) {
            this.action = action;
            this.data = data;
            this.serverManager = serverManager;
        }

        public override void Dispatch() {
            // do the dispatchy
            serverManager.DispatchMessage(action, data);
        }
    }
    
    // An Action is a function without any parameters
    class PullThroughNetworkEvent : NetworkEvent {
        private Action _action;

        public PullThroughNetworkEvent(Action action) {
            _action = action;
        }

        public override void Dispatch() {
            // do the dispatchy
            _action();
        }
    }
    class BasicSendingNetworkEvent : NetworkEvent {
        private string data;
        private WebSocket socket;

        public BasicSendingNetworkEvent(string data, WebSocket socket) {
            this.data = data;
            this.socket = socket;
        }

        public override void Dispatch() {
            // do the dispatchy
            //Debug.Log("Dispatching " + data);
            socket.Send(data);
        }
    }
    
     
    

    private List<NetworkEvent> queuedEvents = new List<NetworkEvent>();

    public void Connect() {
        socket = new WebSocket($"wss://{server}/socket.io/?EIO=3&transport=websocket");

        socket.Log.Level = LogLevel.Trace;
        socket.Log.File = @"C:\Users\solca\RiderProjects\prco204-supine\wslog.txt";

        socket.OnClose += (sender, args) => {
            OnSocketDisconnected?.Invoke(String.Format("[{0}] {1}", args.Code, args.Reason));
            StopNetworkTick();
        };

        socket.OnMessage += (sender, e) => DecodeMessage(e.Data);

        socket.OnOpen += (sender, e) => {
            Debug.Log("Server connection established");
            InvokeRepeating("SendHeartbeat", 25, 25); // Heartbeat every 25s after 25s
            OnSocketConnected?.Invoke();
            //StartNetworkTick();
        };

        socket.OnError += (sender, e) => {
            OnSocketError?.Invoke(e.ToString());
            //StopNetworkTick();
            throw e.Exception;
        };

        socket.Connect();
    }

    private void StartNetworkTick() {
        Debug.Log("Starting network tick");
        NetworkTickCoroutine = StartCoroutine(NetworkTick());
    }

    private IEnumerator NetworkTick() {
        for (;;) {
            //Debug.Log("NETWORK_TICK");
            OnNetworkTickRequest?.Invoke();
            // network tick
            /*
            GameObject player;

            float[] pos = { player.transform.position.x, player.transform.position.z };
            socket.Send(EncodeMessage("player:position", pos));
            yield return new WaitForSeconds((1 / 30f));
            */

            yield return new WaitForSeconds((1 / 30f));
        }
    }

    private void StopNetworkTick() {
        Debug.Log("Stopping network tick");
        StopCoroutine(NetworkTickCoroutine);
    }

    private void SendHeartbeat() {
        socket.Send("2");
    }

    private string EncodeMessage(params object[] args) {
        return "42" + JsonConvert.SerializeObject(args);
    }

    /// <summary>
    /// Messages from socket.io will be handled with engine.io and optionally socket.io
    ///   example data message =
    ///   42{"event": "start", "args": [1, 4, 5]}
    /// </summary>
    /// <param name="input"></param>
    private void DecodeMessage(String input) {
        int enginePacketType = int.Parse(input.Substring(0, 1));
        switch (enginePacketType) {
            case 0:
                break;
            case 3:
                break;
            case 4:
                int socketPacketType = int.Parse(input.Substring(1, 1));

                if (socketPacketType != 2) {
                    Debug.LogWarning("Unknown socket packet type " + socketPacketType);
                    return;
                }
                // only 42 [data message] here

                ArrayList j = JsonConvert.DeserializeObject<ArrayList>(input.Substring(2));
                string eventName = j[0].ToString();
                j.RemoveAt(0);

                if (eventName == "rooms:joined") {
                    queuedEvents.Add(new PullThroughNetworkEvent(StartNetworkTick));
                }
                
                queuedEvents.Add(new BasicNetworkEvent(eventName, j, serverManager));
                
                break;
            default:
                Debug.LogWarning("Unknown engine packet type: " + enginePacketType);
                break;
        }

        //Debug.Log("[MSG] " + input);

        // quick network test 

        /*
        if (input.Contains("rooms:joined")) {
            Debug.Log(input);
        }*/
    }

    public void SendMessage(String action, params object[] args) {
        var data = new object[] {action, args};
        String socketData = EncodeMessage(data);

        try {
            queuedEvents.Add(new BasicSendingNetworkEvent(socketData, socket));
        }
        catch (Exception e) {
            Debug.LogError($"Error whilst queueing a new send message event: {e.Message}");
        }
    }


    private void Awake() {
        serverManager = gameObject.GetComponent<ServerManager>();
    }


    private void Update() {
        if (queuedEvents.Count > 0) {
            List<NetworkEvent> items = queuedEvents.ToList();
            lock (items) ;
            queuedEvents = new List<NetworkEvent>();
            
            Debug.Log($"Event count: {items.Count}");
            try {
                foreach (NetworkEvent item in items) {
                    item.Dispatch();
                }
            }
            catch (Exception e) {
                Debug.LogError(e.Message);
            }
        }
    }
}