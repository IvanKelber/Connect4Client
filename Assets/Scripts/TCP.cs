using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System; 
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCP
{

    public TcpClient socket;

    private NetworkStream stream;
    private ByteBuffer receiveBuffer;

    public delegate void ConnectCallback();
    private ConnectCallback connectCallback;

    private string ip;
    private int port;

    private Thread listenerThread;
    private bool listening = false;
    private ConcurrentQueue<Message> messagesToProcess = new ConcurrentQueue<Message>();
    public bool MessagesNeedProcessing {
        get {
            return !messagesToProcess.IsEmpty;
        }
    }

    public TCP(string ip, int port) {
        this.ip = ip;
        this.port = port;
    }

    public void SetConnectCB(ConnectCallback cb) {
        connectCallback = cb;
    }

    public void Connect()
    {
        socket = new TcpClient();
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
        StartListeningOnThread();
        connectCallback();
    }

    public void StartListeningOnThread() {
        listenerThread = new Thread(new ThreadStart(ListenForResponse));
        listening = true;

        listenerThread.Start();
    }

    public void OnDisconnect() {
        listening = false;
        listenerThread.Abort();
        stream.Close();
        socket.Close();
    }

    private void ListenForResponse() {
        Debug.Log("Listening for response");
        while (listening) {
            while(socket.Available == 0) {
                //wait
                Thread.Sleep(1);
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
                }
                receiveBuffer.Write(ms.ToArray());
                if(receiveBuffer.Count > 0) {
                    OnReceive();
                }
            }
        }
    }

    private void OnReceive()
    {
        Message msg = Message.Deserialize(receiveBuffer);
        messagesToProcess.Enqueue(msg);
    }

    public Message NextMessage() {
        Message msg;
        if(messagesToProcess.TryDequeue(out msg)) {
            return msg;
        }
        throw new InvalidOperationException("No new messages available");
    }

}