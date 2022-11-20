using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Manage the state of game inside Game scene
public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;     // for singleton access

    // public reference
    [Header("Public References")]
    public TMP_Text timerText;
    public GameObject p1Container;
    public TMP_Text p1NameText;
    public TMP_Text p1ScoreText;
    public GameObject p2Container;
    public TMP_Text p2NameText;
    public TMP_Text p2ScoreText;
    public InputProcess inputProcess;
    public Spawner spawner;
    public WordManager wordManager;

    // managing var
    float timer = 0;
    int score = 0;
    string p1Name = "";
    int p1Score = 0;
    string p2Name = "";
    int p2Score = 0;
    
    // other var
    public bool isPlaying = false;
    public bool isMultiplayer = false;

    //----------------------Start and Update-----------------------
    void Awake() {
        // singleton: 1 object at a time contain this component
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        // add single player wordlist
        WordList.AddListResource("wordlist10000", "wordlist10000");
    }

    void Start() {
        // Spawner.instance.StartSinglePlayerSpawnerCR();
        Spawner.instance.StartMultiPlayerSpawnerCR();
    }
    //----------------------Functions-----------------------
    public void SetupSingleplayer(float timer) {
        this.timer = timer;
        this.p1Name = "You";
        this.p2Name = "";
        this.p1Score = 0;
        this.p2Score = 0;

        this.timerText.gameObject.SetActive(true);
        this.p1Container.gameObject.SetActive(true);
        this.p2Container.gameObject.SetActive(false);

        UpdateNameText();
        UpdateScoreText();
    }

    public void SetupMultiplayer(string p1Name, string p2Name, float timer) {
        this.timer = timer;
        this.p1Name = p1Name;
        this.p2Name = p2Name;
        this.p1Score = 0;
        this.p2Score = 0;

        this.timerText.gameObject.SetActive(true);
        this.p1Container.gameObject.SetActive(true);
        this.p2Container.gameObject.SetActive(true);

        UpdateNameText();
        UpdateScoreText();
    }

    public void UpdateNameText() {
        p1NameText.text = p1Name;
        p2NameText.text = p2Name;
    }

    public void UpdateScoreText() {
        p1ScoreText.text = "" + p1Score;
        p2ScoreText.text = "" + p2Score;
    }

    public void UpdateTimerText() {
        timerText.text = "" + timer;
    }

    public void UpdateScores(int p1Score, int p2Score) {
        this.p1Score = p1Score;
        this.p2Score = p2Score;

        UpdateScoreText();
    }

    ///<summary>Trigger the end of the game</summary>
    public static void EndGame() {
        // use stopping time for now
        Time.timeScale = 0;
    }
}
