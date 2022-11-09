using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Newtonsoft.Json;

public class ConnectionManager
{
    // handle received json and runs function accordingly
    public static void Handle(string jsonMsg) 
    {
        // remove "\"
        jsonMsg = jsonMsg.Replace("\\", "");
        // jsonMsg = jsonMsg.Substring(8,(jsonMsg.Length-8));
        // jsonMsg += "$$";

        // convert json msg to obj for data retrieval
        ReceivedMsgInfo msgObj = ReceivedMsgInfo.FromJSON(jsonMsg);

        // if catchMsg is set, then it means that json parsing fail
        if (!msgObj.catchMsg.Equals("")) {  
            // if catchMsg is not empty, then parsing fail
            // simply debug message and return
            Debug.Log("ConnectionManager: Parse failed: Server msg: " + msgObj.catchMsg);
            return;
        }

        // determine what data is received
        if (msgObj.Player.Length > 0) {
            // Receive player info
            
            // extract the scores of players and udpate them in GameManager
            int score1 = msgObj.Player[0];
            int score2 = msgObj.Player[1];
            GameManager.updateScores(score1, score2);

            // console log
            string s = "ConnectionManager: Receive player info: ";
            for (int i = 0; i < msgObj.Player.Length; i++) {
                s += $"id# = {i}, point = {msgObj.Player[i]}   ";
            }
            Debug.Log(s);
        }
        if (msgObj.Words.Length > 0) {
            // Receive new word list

            // check if the Spawner instance is not null
            if(Spawner.instance != null) {
                // put the words into queue
                foreach (string i in msgObj.Words){                
                    Spawner.wordQueue.Enqueue(i);
                }
            }

            // console log
            string s = "ConnectionManager: Receive new word list: ";
            foreach (string i in msgObj.Words) {
                s += $"{i},";
            }
            Debug.Log(s);
        }
        if (!msgObj.WordRemoved.Equals("")) {
            // Receive order to remove word
            
            // call funciton to remove the word
            if(WordManager.instance != null) {
                WordManager.CheckWord(msgObj.WordRemoved, true);
                // Debug.Log($"--{msgObj.WordRemoved}--");
            }

            // console log
            string s = "ConnectionManager: Remove Word: ";
            s += $"--{msgObj.WordRemoved}--";
            Debug.Log(s);
        }
    }

    void Update() {
        Debug.Log(GetLocalIPAddress());
    }

    // sending what the player typed to the server
    public static void deliverMsg(string key, string word) {
        string s = "{";
        if (Client.instance != null) {
            s += "{\"ID\"=" + Client.instance.myId + ",";
        }
        s += "\""+ key + "\":\"" + word + "\"}";
        ClientSend.SendString(s);
        // Debug.Log(s);
    }
/*
{
    "ClientID":0,
    "PlayerReady":1
}

{
    "ClientID":0,
    "PlayerTyped":"word"
}

{
    "ClientID":0,
    "WordExpire":"word"
}

{"Player":{"Player1":{"ID":1,"Point":0},"Player2":{"ID":2,"Point":0}}}
*/
    

    // class for representing received msg from server
    // test string for server: {"newWord":"1word","p1Score":1,"p2Score":10,"removeWord":"3word"}
    // {"words":["James", "annoyed", "with", "Ball"]}
    // {"Player":[{"ID":1,"Point":0},{"ID":2,"Point":0}]}{"Words":["James", "annoyed", "with", "Ball"]}
    public class ReceivedMsgInfo
    {
        public int[] Player = {};
        public string[] Words = {};
        public string WordRemoved = "";
        public string catchMsg = ""; // case there's an error in parsing to json, it is not a json format

        // create ReceiveMsgInfo from JSON
        public static ReceivedMsgInfo FromJSON(string jsonString)
        {
            // JsonConvert.DeserializeObject<ReceivedMsgInfo>(jsonString);
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

    // debugging
    public static string ByteArrayToBinary(byte[] _data) {
        string output = "";
        foreach (Byte b in _data) {
            output += " " + Convert.ToString(b, 2).PadLeft(8, '0');
        }
        return output;
    }
}
