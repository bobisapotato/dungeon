using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WebSocketSharp;
using Newtonsoft.Json;
using TMPro.EditorUtilities;

public class WSConnection : MonoBehaviour {
    public string server;
    private WebSocket socket;
    private Coroutine NetworkTickCoroutine;

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

    private void Connect()
    {
        socket = new WebSocket($"wss://{server}/socket.io/?EIO=3&transport=websocket");


        socket.OnClose += (sender, args) => { OnSocketDisconnected?.Invoke(String.Format("[{0}] {1}", args.Code, args.Reason)); };
        
        socket.OnMessage += (sender, e) => DecodeMessage(e.Data);

        socket.OnOpen += (sender, e) => {
            Debug.Log("Server connection established");
            InvokeRepeating("SendHeartbeat", 25, 25); // Heartbeat every 25s after 25s
            OnSocketConnected?.Invoke();
        };

        socket.OnError += (sender, e) => { OnSocketError?.Invoke(e.ToString()); };

        socket.Connect();
    }

    private void StartNetworkTick()
    {
        Debug.Log("Starting network tick");
        NetworkTickCoroutine = StartCoroutine(nameof(NetworkTick));
    }

    IEnumerator NetworkTick()
    {
        Debug.Log("NetworkTick");
        for (;;) {
            Debug.Log("NETWORK_TICK");
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

    private void DecodeMessage(String input) {
        Debug.Log("[MSG] " + input);

        // quick network test 
        if (input.Contains("rooms:joined")) {
            StartNetworkTick();
        }
    }

    public void SendMessage(String action, params object[] args) {
        var data = new object[] { action, args }; 
        socket.Send(EncodeMessage(data));
    }


    
    private void Start() {
        Connect();
    }
}