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
    private Dictionary<int, ResponseHandler> responseHandlers;

    public bool IsConnected = false;

    [SerializeField]
    private Lobby lobby;

    private void Start()
    {
        tcp = new TCP(ip, port);
        tcp.SetConnectCB(OnConnect);
        ConnectToServer();
    }

    private void Update() {
        if(tcp.MessagesNeedProcessing) {
            StartCoroutine(HandleMessage(tcp.NextMessage()));
        }
    }

    private void ConnectToServer()
    {
        InitializeClientData();

        tcp.Connect();
        
    }

    public void OnConnect() {

        IsConnected = true;
        Debug.Log("Established Connection");
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
            { DEFAULT_HANDLER, DefaultHandle},
            { Message.UpdateLobbyResp, UpdateLobby}
        };
    }

    public void DefaultHandle(Message message) {
        message.GetContentStringList().ForEach(Debug.Log);
        // Debug.Log($"Message from server: {message.GetContentStringList()}");
    }

    public void UpdateLobby(Message message) {
        List<string> players = message.GetContentStringList();
        lobby.SetPlayersInLobby(players);
    }

    public void SetUsername(string username) {
        Message message = new Message(Message.Request, Message.NewPlayerReq, 29);
        message.AddContentString(username);
        tcp.SendData(message);
    }

    public void SendGameRequest(string opponentUsername) {
        Debug.Log("Sending a game request to opponent " + opponentUsername);
    }

}