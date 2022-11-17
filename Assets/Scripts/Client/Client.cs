using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 26950;
    public int myId = 0;
    public TCP tcp;

    private bool isConnected = false;
    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        tcp = new TCP();
    }

    private void OnApplicationQuit() 
    {
        Disconnect();
    }

    public void ConnectToServer()
    {
        InitializeClientData();

        isConnected = true;
        tcp.Connect();
    }

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            Debug.Log("Check 1");
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
            Debug.Log("Check 1.1");
        }

        private void ConnectCallback(IAsyncResult _result)
        {
            Debug.Log("Check 2");
            socket.EndConnect(_result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            receivedData = new Packet();

            Debug.Log("Check 2.1");
            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            Debug.Log("Check 2.2");
            ConnectionManager.MenuSendFirstMessage();
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                    // Debug.Log("send: " + ConnectionManager.ByteArrayToBinary(_packet.ToArray()));
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        { 
            Debug.Log("Check 3");
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    // disconnect
                    instance.Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));
                Debug.Log("Check 3.1");
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                Debug.Log("Check 3.2");
            }
            catch
            {
                // disconnect
                Disconnect();
            }
        }

        private bool HandleData(byte[] _data)
        {   
            // Debug.Log("received: " + ConnectionManager.ByteArrayToBinary(_data));

            /*
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }
            */
            // while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            /*
            while (_packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        packetHandlers[_packetId](_packet);
                    }
                });

                _packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
            */
            byte[] _packetBytes = _data;
            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_packetBytes))
                {
                    int _packetId = 1;
                    packetHandlers[_packetId](_packet);
                }
            });
            return true;
        }

        public void Disconnect() 
        {
            instance.Disconnect();

            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }
    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.welcome, ClientHandle.Handle }
        };
        Debug.Log("Initialized packets.");
    }

    private void Disconnect() 
    {
        if (isConnected) 
        {
            isConnected = false;
            tcp.socket.Close();

            Debug.Log("Disconnected from server.");

            // use for connection debug menu
            if (ConnectionUIManager.instance != null) ConnectionUIManager.instance.OnDisconnectFromServer();
        }
    }
}