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
        if (msgObj.catchMsg != "") {
            // simply debug message
            Debug.Log("Server: " + msgObj.catchMsg);
        } else {    // json parsing success
            if (msgObj.newWord != "") {             // if newWord field exist
                // TODO: check if in play mode/scene
                // TODO: spawn new word
            }
            if (msgObj.p1Score != int.MinValue) {   // if p1Score field exist
                // TODO: check if in play mode/scene
                // TODO: update score
            }
            if (msgObj.p2Score != int.MinValue) {   // if p2Score field exist
                // TODO: check if in play mode/scene
                // TODO: update score
            }
            if (msgObj.removeWord != "") {          // if removeWord field exist
                // TODO: check if in play mode/scene
                // TODO: update score
            }
        }
    }

    

    // class for representing received msg from server
    // test string for server: {"newWord":"1word","p1Score":1,"p2Score":10,"removeWord":"3word"}
    public class ReceivedMsgInfo
    {
        public string newWord;
        public int p1Score = int.MinValue;  //use int.MinValue as the representation of no value
        public int p2Score = int.MinValue;
        public string removeWord;
        
        public string catchMsg; // case there's an error in parsing to json, it is not a json format

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
