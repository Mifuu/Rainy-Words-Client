using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Handle(Packet _packet)
    {
        string _msg = _packet.ReadString();
        
        ConnectionManager.Handle(_msg);
    }

    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();
        
        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();
    }
}