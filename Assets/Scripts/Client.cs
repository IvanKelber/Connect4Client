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
    }

    private void Update() {
        if(tcp.MessagesNeedProcessing) {
            StartCoroutine(HandleMessage(tcp.NextMessage()));
            Debug.Log("Messages need processing");
        }
        if(Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Messages in queue: " + tcp.MessageCount);
        }
    }

    public void ConnectToServer()
    {
        InitializeClientData();

        tcp.Connect();
        
    }

    public void OnConnect() {

        Message message = new Message(Message.Request, Message.NewPlayerReq, 29);
        message.AddContentString(UIManager.instance.usernameField.text);
        tcp.SendData(message);
    }

    public void OnApplicationQuit() {
        OnDisconnect();
    }

    public void OnDisable() {
        OnDisconnect();
    }

    public void OnDisconnect() {
        tcp.OnDisconnect();
    }

    public IEnumerator HandleMessage(Message message) {
        ResponseHandler handler;            
        if(!responseHandlers.TryGetValue(message.ID, out handler)) {
            handler = responseHandlers[DEFAULT_HANDLER];
        }
        handler(message);
        yield return null;
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