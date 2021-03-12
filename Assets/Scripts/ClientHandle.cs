using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class ClientHandle : MonoBehaviour
{
    // public static void Welcome(Packet _packet)
    // {
    //     string _msg = _packet.ReadString();
    //     int _myId = _packet.ReadInt();

    //     Debug.Log($"Message from server: {_msg}");
    //     Client.instance.myId = _myId;
    //     ClientSend.WelcomeReceived();
    // }

    public static void Default(Message message) {
        message.GetContentStringList().ForEach(Debug.Log);
        // Debug.Log($"Message from server: {message.GetContentStringList()}");
    }



}