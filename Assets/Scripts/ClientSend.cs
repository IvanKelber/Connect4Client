using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{

    private Client client;

    void Awake() {
        client = GetComponent<Client>();
    }

    private void SendTCPData(Message message)
    {
        client.tcp.SendData(message);
    }

    public void RequestNewPlayer() {
        Message message = new Message(Message.Request, Message.NewPlayerReq, 29);
        message.AddContentString(UIManager.instance.usernameField.text);
        SendTCPData(message);
    }
}