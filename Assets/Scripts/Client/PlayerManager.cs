using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Newtonsoft.Json;

public class PlayerManager
{
    public static string id = "";   // contain ip address of player for verifying some packets

    // handle received json and runs function accordingly
    public static void Handle(string jsonMsg) 
    {
        // convert json msg to obj for data retrieval
        ReceivedMsgInfo msgObj = ReceivedMsgInfo.FromJSON(jsonMsg);

        // if catchMsg is set, then it means that json parsing fail
        if (!msgObj.catchMsg.Equals("")) {  
            // if catchMsg is not empty, then parsing fail
            // simply debug message and return
            Debug.Log("Parse failed: Server msg: " + msgObj.catchMsg);
            return;
        }

        // parse success
        Debug.Log("Parse success");

        // determine what data is received
        if (msgObj.Player.Length > 0) {
            Debug.Log("dshbdshdskc");
        }
        Debug.Log(msgObj.WordRemoved);
    }

    void Update() {
        Debug.Log(GetLocalIPAddress());
    }
/*
{
    "ClientID":0,
    "PlayerReady":1
}

{
    "ClientID":0,
    "PlayerType":"word"
}

{
    "ClientID":0,
    "WordExpire":"word"
}

{"Player":{"Player1":{"ID":1,"Point":0},"Player2":{"ID":2,"Point":0}}}
*/
    

    // class for representing received msg from server
    // test string for server: {"newWord":"1word","p1Score":1,"p2Score":10,"removeWord":"3word"}
    // {"Player":{"Player1":{"ID":1,"Point":0},"Player2":{"ID":2,"Point":0}}}
    // {"Words":["James", "annoyed", "with", "Ball"]}
    public class ReceivedMsgInfo
    {
        public PlayerReceivedMsgInfo[] Player = {};
        public string[] Words = {};
        public string WordRemoved = "";
        public string catchMsg = ""; // case there's an error in parsing to json, it is not a json format

        // create ReceiveMsgInfo from JSON
        public static ReceivedMsgInfo FromJSON(string jsonString)
        {
            try {   // try parsing JSON, if success return obj that represent that JSON
                return JsonConvert.DeserializeObject<ReceivedMsgInfo>(jsonString);
                //return JsonUtility.FromJson<ReceivedMsgInfo>(jsonString);
            } catch (Exception ex) {    // fail parsing JSON. Put whole msg to catchMsg and return obj
                ReceivedMsgInfo n = new ReceivedMsgInfo();
                n.catchMsg = jsonString;
                return n;
            }
        }
    }

    public class PlayerReceivedMsgInfo
    {
        public int ID;
        public int Point;
    }

    // class for sending json info to server
    public class SendMsgInfo
    {
        string typeWord;
        string wordExpire;

        public string ToJSON()
        {
            return JsonUtility.ToJson(this);
        }
    }

    // get self ip address
    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
}
