using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestSendMsg : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_InputField inputField2;
    public Button input2Button;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && inputField != null) {
            ClientSend.SendString(inputField.text);
        }
    }

    public void SubmitParseTest() {
        PlayerManager.Handle(inputField2.text);
    }
}
