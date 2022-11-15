using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //use Textmesh Pro

[RequireComponent(typeof(TMP_InputField))]
public class InputProcess : MonoBehaviour
{
    TMP_InputField inputField;
    private bool autoActivateInputField = true;

    void Awake() {
        inputField = GetComponent<TMP_InputField> ();
    }

    void Start() {
        inputField.ActivateInputField();
    }

    void Update() {
        // check if for conditions to disable autpActivateInputField
        if (ConnectionUIManager.instance != null && ConnectionUIManager.instance.isOn) {
            autoActivateInputField = false;
        }
        
        // if press [Enter]
        if (Input.GetKeyDown(KeyCode.Return)) {
            // process word in the input field
            ProcessWord(inputField.text);
            // reset input field
            inputField.text = "";
            // reactivate input field incase it deactivate itself after [Enter]
            if (autoActivateInputField) inputField.ActivateInputField();
        }
    }

    //----------------------Processing Entered Word-----------------------
    // on every [Enter]
    void ProcessWord(string word) {
        // Debug.Log(inputField.text);

        // calling deliverMsg to send message in JSON format when the typed word matches
        if(WordManager.CheckWord(word, true)) ConnectionManager.DeliverMsg("playerTyped", word);

    }

    //----------------------Events-----------------------
    // on every key input changed in input field
    public void OnType() {
        WordManager.CheckTyping(inputField.text, Color.red);
    }

    public void OnDeselect() {
        // SO it can never really be deselect/unactivated
        if (autoActivateInputField) inputField.ActivateInputField();
    }
}
