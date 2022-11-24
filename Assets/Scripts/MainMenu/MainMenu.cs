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

    [Header("Player List Self Item Text")]
    public TMP_Text pliSelfText;

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

    public string GetInputName() {
        /*
        string output = "";
        output += nameInputField.text.Equals("");
        */
        return nameInputField.text;
    }

    public void SetPLISelf(string name) {
        pliSelfText.text = name;
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
            if (ConnectionUIManager.instance != null) ConnectionUIManager.instance.ButtonConnect();
            PanelManager.StaticNext("Waiting For Connection Panel");
            GameManager.instance.WaitForConnectionToPlayerList();
        }
    }

    public void ButtonChangeSceneSinglePlayer(int id) {
        if (GameManager.instance != null) {
            GameManager.instance.ChangeSceneSinglePlayer(id);
        }
    }

    public void ButtonRemoveClient() {
        ConnectionManager.DeliverMsg("removeClient", Client.instance.myId);
        Client.instance.Disconnect();
 
    }

    // specific input per panel
    float onlinePlayers_refreshPeriod = 3;
    float onlinePlayers_timeTilRefresh = 0;
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
                if (!ConnectionUIManager.instance.isOn) {
                    nameInputField.ActivateInputField();
                }
                break;
            case "Welcome Panel":
                if (Input.GetKeyDown(KeyCode.Return)) {
                    PanelManager.StaticNext("Online Players Panel");
                }
                break;
            case "Online Players Panel":
                onlinePlayers_timeTilRefresh -= Time.deltaTime;
                if (onlinePlayers_timeTilRefresh < 0) {
                    onlinePlayers_timeTilRefresh = onlinePlayers_refreshPeriod;
                    // send refresh msg
                    ConnectionManager.DeliverMsg("requestPlayerList", Client.instance.myId);
                }
                break;
        }
    }

    public void GoOnlinePlayerPanel() {
        PanelManager.StaticNext("Online Players Panel");
    }
}
