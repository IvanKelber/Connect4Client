using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System; 
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCP
{

    const int LISTEN_TIMEOUT = 10000; // Wait ten seconds before timing out... maybe this is too small


    public static int dataBufferSize = 4096;


    public TcpClient socket;

    private NetworkStream stream;
    private ByteBuffer receiveBuffer;

    public delegate void ConnectCallback();
    private ConnectCallback connectCallback;

    public delegate void ReceiveCallback(Message message);
    private ReceiveCallback receiveCallback;

    private string ip;
    private int port;

    public TCP(string ip, int port) {
        this.ip = ip;
        this.port = port;
    }

    public void SetConnectCB(ConnectCallback cb) {
        connectCallback = cb;
    }

    public void SetReceiveCB(ReceiveCallback cb) {
        receiveCallback = cb;
    }

    public void Connect()
    {
        socket = new TcpClient
        {
            ReceiveBufferSize = dataBufferSize,
            SendBufferSize = dataBufferSize
        };
        receiveBuffer = new ByteBuffer();
        socket.BeginConnect(ip, port, OnConnect, socket);
    }

    public string GetLocalAddress() {
        return ("My local IpAddress is :" + IPAddress.Parse (((IPEndPoint)socket.Client.LocalEndPoint).Address.ToString ()) 
                        + "I am connected on port number " + ((IPEndPoint)socket.Client.LocalEndPoint).Port.ToString ());
    }

    public string GetRemoteAddress() {
        return ("The remote IpAddress is :" + IPAddress.Parse (((IPEndPoint)socket.Client.RemoteEndPoint).Address.ToString ()) 
                        + " I am connected on port number " + ((IPEndPoint)socket.Client.RemoteEndPoint).Port.ToString ());
    }


    public void SendData(Message _message)
    {
        try
        {
            if (socket != null)
            {
                byte[] serializedMessage = _message.Serialize();
                stream.BeginWrite(serializedMessage, 0, serializedMessage.Length, OnWriteComplete, null);

            }
        }
        catch (Exception _ex)
        {
        }
    }

    public void OnWriteComplete(IAsyncResult _result) {
        stream.EndWrite(_result);
        ListenForResponse();
    }

    private void OnConnect(IAsyncResult _result)
    {
        socket.EndConnect(_result);

        if (!socket.Connected)
        {
            return;
        }
        // Connection just completed
        stream = socket.GetStream();

        connectCallback();
    }

    public void ListenForResponse() {
        Debug.Log("Listening for response");
        int waitedMilliseconds = 0;
        while(socket.Available == 0 && waitedMilliseconds < LISTEN_TIMEOUT) {
            // Spin until socket gets some data available
            Thread.Sleep(1);
            waitedMilliseconds++;
        }
        if (waitedMilliseconds >= LISTEN_TIMEOUT) {
            Debug.Log("CLIENT LISTENER TIMED OUT");
            return;
        }
        // If there's data available then we read a byte
        using (MemoryStream ms = new MemoryStream())
        {

            string str;

            while(socket.Available > 0) {


                byte[] data = new byte[socket.Available];
                int numBytesRead = 0;
                int bytesRead = 0;
                while ( bytesRead < socket.Available &&
                    (numBytesRead = stream.Read(data, 0, data.Length)) > 0)
                {
                    bytesRead += numBytesRead;
                    ms.Write(data, 0, numBytesRead);
                }
                // string s = "[";
                // foreach(byte b in ms.ToArray()) {
                //     s += b + " ";
                // }
                // s += "]";
                // Debug.Log("Message: " + s);
            }
            receiveBuffer.Write(ms.ToArray());
        }
        OnReceive();
    }

    private void OnReceive()
    {
        // receiveBuffer.Catchup();
        Message msg = Message.Deserialize(receiveBuffer);
        receiveCallback(msg);
        Debug.Log(msg.Content);
    }




//     private bool HandleData(byte[] _data)
//     {
//         int _packetLength = 0;

//         receivedData.SetBytes(_data);

//         if (receivedData.UnreadLength() >= 4)
//         {
//             _packetLength = receivedData.ReadInt();
//             if (_packetLength <= 0)
//             {
//                 return true;
//             }
//         }

//         while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
//         {
//             byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
//             ThreadManager.ExecuteOnMainThread(() =>
//             {
//                 using (Packet _packet = new Packet(_packetBytes))
//                 {
//                     int _packetId = _packet.ReadInt();
//                     packetHandlers[_packetId](_packet);
//                 }
//             });

//             _packetLength = 0;
//             if (receivedData.UnreadLength() >= 4)
//             {
//                 _packetLength = receivedData.ReadInt();
//                 if (_packetLength <= 0)
//                 {
//                     return true;
//                 }
//             }
//         }

//         if (_packetLength <= 1)
//         {
//             return true;
//         }

//         return false;
//     }
}