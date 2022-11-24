using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //use Textmesh Pro

[RequireComponent(typeof(TMP_InputField))]
public class InputProcess : MonoBehaviour
{
    TMP_InputField inputField;
    public bool autoActivateInputField = true;

    public Color onTypeColor;

    void Awake() {
        inputField = GetComponent<TMP_InputField> ();
    }

    void Start() {
        inputField.ActivateInputField();
    }

    void Update() {
        // check if for conditions to disable autoActivateInputField
        if (ConnectionUIManager.instance != null && ConnectionUIManager.instance.isOn) {
            autoActivateInputField = false;
        } else {
            autoActivateInputField = true;
        }
        
        // if press [Enter]
        if (Input.GetKeyDown(KeyCode.Return)) {
            // act like button
            ButtonProcessWord();
        }
    }

    //----------------------Processing Entered Word-----------------------
    // on every [Enter]
    void ProcessWord(string word) {
        // Debug.Log(inputField.text);

        if (GameManager.nextSceneMode == GameManager.NextSceneMode.Multi) {
            // calling deliverMsg to send message in JSON format when the typed word matches
            if (WordManager.CheckWord(word, false)) {
                ConnectionManager.DeliverMsg("playerTyped", word);
                SFXManager._PlaySFX("Right1", gameObject);
            } else {
                SFXManager._PlaySFX("Wrong1", gameObject);
            }
        } else {
            if (GameManager.instance.singleModeID == 3) {
                // netcentric mode
                if (WordManager.CheckWordNetcentric(word, true)) {
                    PlayManager.AddSinglePlayerScore();
                    SFXManager._PlaySFX("Right1", gameObject);
                } else {
                    SFXManager._PlaySFX("Wrong1", gameObject);
                }
            } else {
                // other single player modes
                if (WordManager.CheckWord(word, true)) {
                    PlayManager.AddSinglePlayerScore();
                    SFXManager._PlaySFX("Right1", gameObject);
                } else {
                    SFXManager._PlaySFX("Wrong1", gameObject);
                }
            }
        }

        WordManager.CheckTyping("I am very sleepy", onTypeColor);

    }

    public void ButtonProcessWord() {
        // process word in the input field
        ProcessWord(inputField.text);
        // reset input field
        inputField.text = "";
        // reactivate input field incase it deactivate itself after [Enter]
        if (autoActivateInputField) inputField.ActivateInputField();
    }

    //----------------------Events-----------------------
    // on every key input changed in input field
    public void OnType() {
        if (GameManager.instance.singleModeID == 3) {
            WordManager.CheckTypingNetcentric(inputField.text, onTypeColor);
        } else {
            WordManager.CheckTyping(inputField.text, onTypeColor);
        }
        SFXManager._PlaySFX("Type1", gameObject);
    }

    public void OnDeselect() {
        // SO it can never really be deselect/unactivated
        if (autoActivateInputField) inputField.ActivateInputField();
    }
}
