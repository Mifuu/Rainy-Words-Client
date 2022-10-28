using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSendMsg : MonoBehaviour
{
    void Start() {
        Debug.Log("Try Connect...");
        ClientSend.SendString("fuckkkkkkkkk");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) {
            Debug.Log("Try Connect...");
            ClientSend.SendString("fuckkkkkkkkk");
        }
    }
}
