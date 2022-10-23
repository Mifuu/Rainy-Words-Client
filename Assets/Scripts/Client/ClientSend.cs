using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    public static void SendString(string msg)
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            //_packet.Write(Client.instance.myId);
            _packet.Write(msg);

            SendTCPData(_packet);
        }
    }

    public static void WelcomeReceived()
    {
        SendString(UIManager.instance.usernameField.text);
    }
}