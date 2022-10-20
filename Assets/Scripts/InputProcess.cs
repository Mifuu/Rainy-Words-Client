using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //use Textmesh Pro

[RequireComponent(typeof(TMP_InputField))]
public class InputProcess : MonoBehaviour
{
    TMP_InputField inputField;

    void Awake() {
        inputField = GetComponent<TMP_InputField> ();
    }

    void Start() {
        inputField.ActivateInputField();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            ProcessWord(inputField.text);
            inputField.text = "";
            inputField.ActivateInputField();
        }
    }

    //----------------------Processing Entered Word-----------------------
    void ProcessWord(string word) {
        Debug.Log(inputField.text);
        Word.CheckWord(word);
    }

    //----------------------Events-----------------------
    public void OnType() {
        Word.CheckTyping(inputField.text, Color.red);
    }

    public void OnDeselect() {
        // SO it can never really be deselect/unactivated
        inputField.ActivateInputField();
    }
}
