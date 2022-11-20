using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConnectionUIManager : MonoBehaviour
{
    // singleton
    public static ConnectionUIManager instance;

    [Header("Fields")]
    public TMP_InputField usernameField;

    [Header("Debug Connection Menu")]
    [SerializeField] private GameObject debugConnectionMenuPanel;
    public bool isOn = false;       // if debug panel is on
    private bool isConnected = false;

    [Header("Debug Connection Field")]
    [SerializeField] private TMP_InputField ipTMP;
    [SerializeField] private TMP_InputField portTMP;
    [SerializeField] public TMP_InputField idTMP;
    [SerializeField] private Button connectButton;
    [SerializeField] private Button disconnectButton;

    [Header("Debug Send MSG Field")]
    [SerializeField] private GameObject sendMsgFieldPanel;
    [SerializeField] private TMP_InputField sendMsgField;
    [SerializeField] private Button sendMsgButton;

    [Header("Debug Simulate Receiving MSG")]
    [SerializeField] private GameObject receiveMsgFieldPanel;
    [SerializeField] private TMP_InputField receiveMsgField;
    [SerializeField] private Button receiveMsgButton;

    [Header("Debug Log")]
    [SerializeField] private TMP_Text debugLogTMP;
    static string myLog = "";
    string output;
    string stack;

    //---------------------Start-----------------------
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start() {
        debugConnectionMenuPanel.SetActive(isOn);

        if (Client.instance != null) {
            ipTMP.text = Client.instance.ip;
            portTMP.text = "" + Client.instance.port;
            idTMP.text = "" + Client.instance.myId;
        }
    }

    //---------------------Update-----------------------
    private void Update() {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Semicolon)) {
            isOn = !isOn;
            UpdateDebugMenu();
        }
    }
    
    private void UpdateDebugMenu() {
        debugConnectionMenuPanel.SetActive(isOn);

        if (!isOn) return;

        ipTMP.interactable = true;
        portTMP.interactable = true;
        idTMP.interactable = false;
        connectButton.interactable = true;
        disconnectButton.interactable = true;
    }

    //---------------------On-----------------------
    public void OnConnectToServer() {
        isConnected = true;
        UpdateDebugMenu();
    }

    public void OnDisconnectFromServer() {
        isConnected = false;
        UpdateDebugMenu();
    }

    //---------------------Button-----------------------
    public void ButtonConnect()
    {   
        // override Client.cs ip and port
        try {
            string ip = ipTMP.text;
            int port = int.Parse(portTMP.text);
            // int id = int.Parse(portTMP.text);
            if (Client.instance != null) {
                // if ip and port of this instance is not empty, override Client.cs
                if (!ip.Equals("")) Client.instance.ip = ip;
                if (port != 0) Client.instance.port = port;
                // if (id != 0) Client.instance.myId = id;
            }
        } catch (Exception e) {
            Debug.Log("Invalid value of ip or port");
        }

        Client.instance.ConnectToServer();
    }

    public void ButtonDisconnect()
    {   
        if (Client.instance?.tcp != null) Client.instance.tcp.Disconnect();
    }

    public void ButtonSendMsg() {
        ClientSend.SendString(sendMsgField.text);
    }

    public void ButtonReceiveMsg() {
        ConnectionManager.Handle(receiveMsgField.text);
    }

    //---------------------Call-----------------------
    void OnEnable() {
        Application.logMessageReceived += Log;
    }

    void OnDisable() {
        Application.logMessageReceived -= Log;
    }

    //---------------------Function-----------------------
    //https://answers.unity.com/questions/125049/is-there-any-way-to-view-the-console-in-a-build.html
    public void Log(string logString, string stackTrace, LogType type) {
        output = logString;
        stack = stackTrace;
        myLog = myLog + "\n" + output;

        if (myLog.Length > 5000) {
            myLog = myLog.Substring(0, 4000);
        }

        debugLogTMP.text = myLog;
    }
}
