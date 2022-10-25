using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static void Handle(string jsonMsg) 
    {
        // convert json msg to obj for data retrieval
        ReceivedMsgInfo msgObj = ReceivedMsgInfo.FromJSON(jsonMsg);

        // if catchMsg is set, then it means that json parsing fail
        if (!msgObj.catchMsg.Equals("")) {
            // simply debug message
            Debug.Log("Server: " + msgObj.catchMsg);
        } else {    // json parsing success
            Debug.Log("test: " + JsonUtility.ToJson(msgObj));
        }
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
*/
    

    // class for representing received msg from server
    // test string for server: {"newWord":"1word","p1Score":1,"p2Score":10,"removeWord":"3word"}
    // {"Player":{"Player1":{"ID":1,"Point":0},"Player2":{"ID":2,"Point":0}}}
    public class ReceivedMsgInfo
    {
        public Dictionary<string, PlayerReceivedMsgInfo> Player = new Dictionary<string, PlayerReceivedMsgInfo> ();
        
        public string catchMsg = ""; // case there's an error in parsing to json, it is not a json format

        // create ReceiveMsgInfo from JSON
        public static ReceivedMsgInfo FromJSON(string jsonString)
        {
            try {   // try parsing JSON, if success return obj that represent that JSON
                return JsonUtility.FromJson<ReceivedMsgInfo>(jsonString);

                // for debugging
                // ReceivedMsgInfo n = JsonUtility.FromJson<ReceivedMsgInfo>(jsonString);
                // Debug.Log($"newWord = \"{n.newWord}\", p1Score = {n.p1Score}, p2Score = {n.p2Score}, removeWord = \"{n.removeWord}\"");
                // return n;
            } catch (Exception ex) {    // fail parsing JSON. Put whole msg to catchMsg and return obj
                ReceivedMsgInfo n = new ReceivedMsgInfo();
                n.catchMsg = jsonString;
                return n;
            }
        }
    }

    public class PlayerReceivedMsgInfo
    {
        int ID;
        int Point;
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
}
