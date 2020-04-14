using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WebSocketSharp;
using Newtonsoft.Json;

public class WSConnection : MonoBehaviour
{
    private WebSocket socket;

    public GameObject serverManagerObj;

    public static string roomCode;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting");
        socket = new WebSocket("wss://research.supine.dev:3018/socket.io/?EIO=3&transport=websocket");
        socket.OnMessage += (sender, e) => {
            //Debug.Log("New message from controller: " + e.Data);
            DecodeMessage(e.Data);
        };
        
            //socket.Send("hi");

            socket.OnOpen += (sender, e) => {
                Debug.Log("Opened");
                InvokeRepeating("SendHeartbeat", 25, 25);

                socket.Send(EncodeMessage("rooms:create"));
                // "42[\"rooms:create\"]"

                StartCoroutine("NetworkTick");
            };
            socket.OnClose += (sender, e) => {
                Debug.Log("Closed");
                Debug.LogError(e.ToString());
            };
            socket.OnError += (sender, e) => {
                Debug.LogError(e.ToString());
            };

        socket.Connect();


    }


    IEnumerator NetworkTick() {
        for (;;) {
            // network tick
            float[] pos = { gameObject.transform.position.x, gameObject.transform.position.z };
            socket.Send(EncodeMessage("player:position", pos));
            yield return new WaitForSeconds((1 / 30f));
        }
    }

    string EncodeMessage(params object[] args) {
        return "42" + JsonConvert.SerializeObject(args);
    }

    void DispatchEvent(string eventName, ArrayList args) {
        //Debug.Log("EVENT:: " + eventName);
        
        foreach (object arg in args) {
            //Debug.Log(arg.ToString());
        }

        if (eventName == "game:action") {
            Debug.Log(args[0].ToString());
            if (args[0].ToString() == "beep") {
                gameObject.transform.position = new Vector3(0, 0, 0);
            }
            if (args[0].ToString() == "boop") {
                gameObject.transform.position = new Vector3(1, 1, 0);
            }
            if (args[0].ToString() == "bomb") {
                ServerManager srv = serverManagerObj.GetComponent<ServerManager>();
                srv.DecodeMessage((float)args[1], (float)args[2], (string)args[0]);
            }
        }
        if (eventName == "rooms:joined") {
            Debug.Log(eventName + " - " + args[0]);
            roomCode = args[0].ToString();
        }

    }


    void SendHeartbeat() {
        socket.Send("2");
    }

    void DecodeMessage(String input) {
        int enginePacketType = int.Parse(input.Substring(0, 1));

        switch (enginePacketType) {
            case 0:
                break;
            case 3:
                break;
            case 4:
                int socketPacketType = int.Parse(input.Substring(1, 1));

                switch (socketPacketType) {
                    case 0:
                        break;
                    case 2:

                        string data = input.Substring(2);
                        //Debug.Log("Event data: " + data);

                        ArrayList j = JsonConvert.DeserializeObject<ArrayList>(data);
                        

                        string eventName = j[0].ToString();
                        j.RemoveAt(0);
                        try {
                            DispatchEvent(eventName, j);
                        }
                        catch (Exception e ) {
                            Debug.LogError(e.Message);
                        }
                        
                        // do stuff here
                        break;

                    default:
                        Debug.LogWarning("Unknown socket packet type:" + socketPacketType.ToString());
                        break;
                }

                break;
            default:
                Debug.LogWarning("Unknown engine packet type: " + enginePacketType.ToString());
                break;
        }
        
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
