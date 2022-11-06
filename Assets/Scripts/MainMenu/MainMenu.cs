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

    public void ButtonQuit() {
        Application.Quit();
    }

    public void ButtonCheckName() {
        // check name
        if (nameInputField.text.Equals("")) {
            // TODO: name too short notification
        } else {
            // TODO: Set name and send info?
            nameText.text = nameInputField.text;
            PanelManager.StaticNext("Welcome Panel");
            PanelManager.StaticNext("Online Players Panel", 4);
        }
    }


}
