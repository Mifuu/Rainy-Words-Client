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
    // handle Client trying to send first message, will be possible if in menu and name input is filled
    public static void SendFirstConnectionMessage() {
        if (MainMenu.instance != null) {
            string name = MainMenu.instance.GetInputName();
            if (!name.Equals("")) {
                DeliverMsg("newClient",name);
            } else {
                Debug.Log("ConnectionManager: ERROR input is empty!");
            }
        } else {Debug.Log("ConnectionManager: ERROR no MainMenu.instance is found!");}
    }

    // handle received json and runs function accordingly
    public static void Handle(string jsonMsg) 
    {
        // remove "\"
        jsonMsg = jsonMsg.Replace("\\", "");
        // jsonMsg = jsonMsg.Substring(8,(jsonMsg.Length-8));
        // jsonMsg += "$$";
        Debug.Log("Received: " + jsonMsg);

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
        if (msgObj.player.Length > 0) {
            // Receive player info
            // Debug log
            string s = "ConnectionManager: Receive player info: ";
            for (int i = 0; i < msgObj.player.Length; i++) {
                s += $"id# = {i}, point = {msgObj.player[i]}   ";
            }
            Debug.Log(s);
            
            // extract the scores of players and udpate them in GameManager
            int score1 = msgObj.player[0];
            int score2 = msgObj.player[1];
            // GameManager.updateScores(score1, score2);
        }
        if (msgObj.words.Length > 0) {
            // Receive new word list
            // Debug log
            string s = "ConnectionManager: Receive new word list: ";
            foreach (string i in msgObj.words) {
                s += $"{i},";
            }
            Debug.Log(s);

            // check if the Spawner instance is not null
            if(Spawner.instance != null) {
                // put the words into queue
                foreach (string i in msgObj.words){                
                    Spawner.wordQueue.Enqueue(i);
                }
            }
        }
        if (!msgObj.wordRemoved.Equals("")) {
            // Receive order to remove word
            // Debug log
            string s = "ConnectionManager: Remove Word: ";
            s += $"--{msgObj.wordRemoved}--";
            Debug.Log(s);
            
            // call funciton to remove the word
            if(WordManager.instance != null) {
                WordManager.CheckWord(msgObj.wordRemoved, true);
                // Debug.Log($"--{msgObj.WordRemoved}--");
            }
        }
        if (msgObj.playerList.Length > 0) {
            // Received player list
            // Debug log
            string s = "ConnectionManager: Receive Player List: ";
            foreach (PlayerListItemReceived p in msgObj.playerList) {
                s += $"(id:{p.id},name:{p.name},isBusy:{p.isBusy}),";
            }
            Debug.Log(s);

            // msg to list and update
            List<(int id, string name, bool isBusy)> currentPlayers = new List<(int id, string name, bool isBusy)>();
            foreach (PlayerListItemReceived p in msgObj.playerList) {
                currentPlayers.Add((p.id, p.name, p.isBusy));
            }
            PlayerList.UpdateAllPlayerListItem(currentPlayers);

            // tell GameManager
            if (GameManager.instance != null) GameManager.instance.ReceivedPlayerList();
        }
        if (msgObj.assignID != int.MaxValue) {
            // Received assignID
            // Debug log
            Debug.Log("ConnectionManager: Receive AssignID: " + msgObj.assignID);

            // set id
            if (Client.instance != null) Client.instance.myId = msgObj.assignID;
            else {Debug.Log("ConnectionManager: Can't find Client!");}

            // tell GameManager
            if (GameManager.instance != null) GameManager.instance.ReceivedAssignID();

            // send back
            DeliverMsg("requestPlayerList", Client.instance.myId);
        }
        if (msgObj.scoreList.Length > 0) {
            // Received score list
            // Debug log
            string s = "ConnectionManager: Receive Score List: ";
            foreach (ScoreReceived c in msgObj.scoreList) {
                s += $"(id:{c.id},score:{c.score}),";
            }
            Debug.Log(s);

            // tell PlayManager
            if (PlayManager.instance != null) {
                PlayManager.instance.UpdateScores(msgObj.scoreList[0].id, msgObj.scoreList[0].score, msgObj.scoreList[1].id, msgObj.scoreList[1].score);
            }
        }
        if (msgObj.matchStart.Length > 0) {
            // Received matchStart
            // Debug log
            string s = "ConnectionManager: Receive Match Start: ";
            foreach (int i in msgObj.matchStart) {
                s += i + ",";
            }
            Debug.Log(s);

            // tell GameManager
            if (GameManager.instance != null) GameManager.instance.ReceivedMatchStart(msgObj.matchStart);
        }
        if (msgObj.matchRequest.Length > 0) {
            // Received matchRequest
            // Debug log
            string s = "ConnectionManager: Receive Match Request from: " + msgObj.matchRequest[0];
            Debug.Log(s);

            // tell PlayerList
            PlayerList.GotMatchRequest(msgObj.matchRequest[0]);
        }

    }

    // sending what the player typed to the server
    public static void DeliverMsg(string key, string word) {
        string s = "{";
        /*
        if (Client.instance != null) {
            s += "{\"ID\"=" + Client.instance.myId + ",";
        }
        */
        s += "\""+ key + "\":\"" + word + "\"}";
        ClientSend.SendString(s);
        // Debug.Log(s);
    }
    public static void DeliverMsg(string key, int word) {
        string s = "{";
        s += "\""+ key + "\":" + word + "}";
        ClientSend.SendString(s);
    }
    public static void DeliverMsg(string msg) {
        ClientSend.SendString(msg);
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
*/
    

    // NEW CONFIRMED RECEIVABLE
    // {"player":[{"id":1,"point":0},{"id":2,"point":0}]}
    // {"words":["I", "am", "so", "fuking", "sleepy"]}
    // {"wordRemoved":"cancer"}
    // {"playerList":[{"id":0,"name":"Alice","isBusy":false},{"id":1,"name":"Bob","isBusy":true},{"id":3,"name":"Trudy","isBusy":true}]}
    // {"scoreList":[{"id":0,"name":"Alice","score":100},{"id":3,"name":"Trudy","score":99}]}
    // {"assignID":123}
    public class ReceivedMsgInfo
    {
        public int[] player = {};
        public string[] words = {};
        public string wordRemoved = "";
        public PlayerListItemReceived[] playerList = {};
        public ScoreReceived[] scoreList = {};
        public int assignID = int.MaxValue;
        public int[] matchStart = {};
        public int[] matchRequest = {};
        public string catchMsg = ""; // case there's an error in parsing to json, it is not a json format

        // create ReceiveMsgInfo from JSON
        public static ReceivedMsgInfo FromJSON(string jsonString)
        {
            // JsonConvert.DeserializeObject<ReceivedMsgInfo>(jsonString);
            try {   // try parsing JSON, if success return obj that represent that JSON
                return JsonConvert.DeserializeObject<ReceivedMsgInfo>(jsonString);
                //return JsonUtility.FromJson<ReceivedMsgInfo>(jsonString);
            } catch (Exception e) {    // fail parsing JSON. Put whole msg to catchMsg and return obj
                ReceivedMsgInfo n = new ReceivedMsgInfo();
                n.catchMsg = jsonString;
                return n;
            }
        }
    }

    public class PlayerListItemReceived
    {
        public int id;
        public string name;
        public bool isBusy;
    }

    public class ScoreReceived
    {
        public int id;
        public int score;
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
