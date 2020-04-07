using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WebSocketSharp;
using Newtonsoft.Json;

public class WSConnection : MonoBehaviour
{
    private string server;
    private WebSocket socket;
    private Coroutine NetworkTickCoroutine;

    public WSConnection(string server) {
        this.server = server;
    }

    private void Connect()
    {
        this.socket = new WebSocket($"wss://wss://{server}/socket.io?EIO=3&transport=websocket");

        socket.OnMessage += (sender, e) => DecodeMessage(e.Data);

        socket.OnOpen += (sender, e) => {
            Debug.Log("Server connection established");
            InvokeRepeating("SendHeartbeat", 25, 25); // Heartbeat every 25s after 25s
        };
    }

    private void StartNetworkTick()
    {
        NetworkTickCoroutine = StartCoroutine("NetworkTick");
    }

    IEnumerator NetworkTick()
    {
        for (;;) {
            // network tick
            /*
            GameObject player;

            float[] pos = { player.transform.position.x, player.transform.position.z };
            socket.Send(EncodeMessage("player:position", pos));
            yield return new WaitForSeconds((1 / 30f));*/
        }
    }

    private void StopNetworkTick() {
        StopCoroutine(NetworkTickCoroutine);
    }

    private void SendHeartbeat() {
        socket.Send("2");
    }

    private string EncodeMessage(params object[] args) {
        return "42" + JsonConvert.SerializeObject(args);
    }

    private void DecodeMessage(String input) {

    }


}