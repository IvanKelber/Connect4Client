using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class Client : MonoBehaviour
{
    const int DEFAULT_HANDLER = -1;

    public string ip = "127.0.0.1";
    public int port = 26950;
    public int myId = 0;
    public TCP tcp;

    private delegate void ResponseHandler(Message _message);
    private static Dictionary<int, ResponseHandler> responseHandlers;

    private void Start()
    {
        tcp = new TCP(ip, port);
        tcp.SetConnectCB(OnConnect);
        tcp.SetReceiveCB(HandleMessage);
    }

    private void Update() {
            // Debug.Log("Update");
        
    }

    public void ConnectToServer()
    {
        InitializeClientData();

        tcp.Connect();
        
    }

    public void OnConnect() {
        Debug.Log(tcp.GetLocalAddress());
        Debug.Log(tcp.GetRemoteAddress());
        Message message = new Message(Message.Request, Message.NewPlayerReq, 29);
        message.AddContentString(UIManager.instance.usernameField.text);
        tcp.SendData(message);
    }

    public void HandleMessage(Message message) {
        ThreadManager.ExecuteOnMainThread( () => {

            ResponseHandler handler;            
            if(!responseHandlers.TryGetValue(message.ID, out handler)) {
                handler = responseHandlers[DEFAULT_HANDLER];
            }
            handler(message);
        });
    }

    private void InitializeClientData()
    {
        responseHandlers = new Dictionary<int, ResponseHandler>()
        {
            { DEFAULT_HANDLER,             ClientHandle.Default},
        };
        Debug.Log("Initialized packets.");
    }
}