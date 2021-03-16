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

    private string username;

    [HideInInspector]
    public bool IsConnected = false;

    [SerializeField]
    private Lobby lobby;

    [SerializeField]
    private WaitPopup waitPopup;    
    
    [SerializeField]
    private ChallengeProposalPopup challengeProposalPopup;

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


    // ========================  REQUEST METHODS  ===========================

    public void SetUsername(string username) {
        Message message = new Message(Message.Request, 
                                      Message.NewPlayerReq, 
                                      Message.DEFAULT_CONTENT_DELIMITER);
        message.AddContentString(username);
        this.username = username;
        tcp.SendData(message);
    }

    public void SendChallengeProposalReq(string opponentUsername) {
        if(opponentUsername == username) {
            Debug.Log("Challenging yourself. TODO check against this");
        }
        Debug.Log("Sending a challenge proposal to opponent " + opponentUsername);
        Message message = new Message(Message.Request, 
                                      Message.ChallengePlayerReq, 
                                      Message.DEFAULT_CONTENT_DELIMITER);
        message.AddContentString(opponentUsername);
        tcp.SendData(message);
    }

    public void AcceptChallengeProposal(string opponentUsername) {
        Message message = new Message(Message.Request, 
                                      Message.ProposalAnswerReq, 
                                      Message.DEFAULT_CONTENT_DELIMITER);
        message.AddContentByte(Message.TRUE_BYTE); // True
        message.AddContentString(opponentUsername);
        message.AddContentString(username);
        tcp.SendData(message);
    }

    public void RejectChallengeProposal(string opponentUsername) {
        Message message = new Message(Message.Request, 
                                      Message.ProposalAnswerReq, 
                                      Message.DEFAULT_CONTENT_DELIMITER);
        message.AddContentByte(Message.FALSE_BYTE); // False
        message.AddContentString(opponentUsername);
        tcp.SendData(message);
    }

    public void CancelChallengeProposal(string opponentUsername) {
        Message message = new Message(Message.Request, 
                                      Message.CancelProposalReq, 
                                      Message.DEFAULT_CONTENT_DELIMITER);
        message.AddContentString(opponentUsername);
        tcp.SendData(message);
    }

    // ========================  RESPONSE HANDLERS  ===========================
    public void DefaultHandle(Message message) {
        message.GetContentStringList().ForEach(Debug.Log);
    }

    public void UpdateLobbyHandle(Message message) {
        List<string> players = message.GetContentStringList();
        lobby.SetPlayersInLobby(players);
    }

    public void WaitForChallengeResponseHandle(Message message) {
        waitPopup.Popup(message.GetContentStringList()[0]);
    }

    public void ChallengeProposalHandle(Message message) {
        challengeProposalPopup.Popup(message.GetContentStringList()[0]);
    }

    public void StartGameHandle(Message message) {
        Debug.Log("Starting game!");
    }

    public void RejectedProposalHandle(Message message) {
        waitPopup.Reject();
    }

    public void ProposalCanceledHandle(Message message) {
        challengeProposalPopup.Cancel();
    }



    private void InitializeClientData()
    {
        responseHandlers = new Dictionary<int, ResponseHandler>()
        {
            { DEFAULT_HANDLER, DefaultHandle},
            { Message.UpdateLobbyResp, UpdateLobbyHandle},
            { Message.WaitForChallengeResp, WaitForChallengeResponseHandle},
            { Message.ChallengeProposalResp, ChallengeProposalHandle},
            { Message.StartGameResp, StartGameHandle},
            { Message.ChallengeRejectedResp, RejectedProposalHandle},
            { Message.CancelProposalResp, ProposalCanceledHandle}
        };
    }
}