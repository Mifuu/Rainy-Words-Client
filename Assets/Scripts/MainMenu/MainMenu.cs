using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    // singleton
    public static MainMenu instance;

    // variables
    [Header("Input Fields")]
    public TMP_InputField nameInputField;

    [Header("Show Fields")]
    public TMP_Text nameText;

    void Awake() {
        // singleton
        if (instance == null) instance = this;
        else Destroy(this);
    }

    void Start() {
        PanelManager.StaticDisableAllNotCurrent();
    }

    void Update() {
        // specific input per panel update
        SpecificInputUpdate();
    }

    public void SendFirstConnectionMessage() {
        if (!nameInputField.text.Equals("")) {
            ConnectionManager.DeliverMsg("newClient", nameInputField.text);
        }
    }

    public void ButtonQuit() {
        Application.Quit();
    }

    public void ButtonCheckName() {
        // check name
        if (nameInputField.text.Equals("")) {
            // TODO: name too short/invalid notification
        } else {
            // TODO: Set name and send info?
            nameText.text = nameInputField.text;
            PanelManager.StaticNext("Welcome Panel");
        }
    }

    // specific input per panel
    void SpecificInputUpdate() {
        // check if PanelManager instance is null
        if (PanelManager.instance == null) return;

        // get current panel
        PanelManager.Panel current = PanelManager.instance.panelStack.Peek();
        
        switch (current.name) {
            case "Enter Name Panel":
                if (Input.GetKeyDown(KeyCode.Return)) {
                    // if enter
                    ButtonCheckName();
                }
                break;
            case "Welcome Panel":
                if (Input.GetKeyDown(KeyCode.Return)) {
                    PanelManager.StaticNext("Online Players Panel");
                }
                break;
        }
    }
}
