using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerListItem : MonoBehaviour
{
    public int id = 0;
    new public string name = "";
    public enum State{Busy, Match, Confirm, Wait, Retain}
    public State state = State.Match;

    // references
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text busyText;
    [SerializeField] private TMP_Text waitText;
    [SerializeField] private Button matchButton;
    [SerializeField] private Button confirmButton;

    void Start() {
        SetState(State.Match);
    }

    public void SetName(string name) {
        this.name = name;
        nameText.text = name;
    }

    public void SetID(int id) {
        this.id = id;
    }

    /// <summary> set state and act accordingly </summary>
    public void SetState(State state) {
        switch (state) {
            case State.Busy:
                busyText.gameObject.SetActive(true);
                waitText.gameObject.SetActive(false);
                matchButton.gameObject.SetActive(false);
                confirmButton.gameObject.SetActive(false);
                break;
            case State.Match:
                busyText.gameObject.SetActive(false);
                waitText.gameObject.SetActive(false);
                matchButton.gameObject.SetActive(true);
                confirmButton.gameObject.SetActive(false);
                break;
            case State.Confirm:
                busyText.gameObject.SetActive(false);
                waitText.gameObject.SetActive(false);
                matchButton.gameObject.SetActive(false);
                confirmButton.gameObject.SetActive(true);
                break;
            case State.Wait:
                busyText.gameObject.SetActive(false);
                waitText.gameObject.SetActive(true);
                matchButton.gameObject.SetActive(false);
                confirmButton.gameObject.SetActive(false);
                break;
            case State.Retain:
                break;
        }
        this.state = state;
    }

    public void ButtonMatch() {
        // TODO: send match request according to protocol
    }

    public void ButtonConfirm() {
        // TODO: send match accept/request according to protocol
    }
}
