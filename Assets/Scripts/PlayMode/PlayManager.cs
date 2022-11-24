using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    public TMP_Text p1ScoreEndgame;
    public TMP_Text p1ScoreEndgameSingle;
    public GameObject p2Container;
    public TMP_Text p2NameText;
    public TMP_Text p2ScoreText;
    public TMP_Text p2ScoreEndgame;
    public InputProcess inputProcess;
    public Spawner spawner;
    public WordManager wordManager;

    // managing var
    static float timer = 0;
    static string p1Name = "";
    static int p1Score = 0;
    static string p2Name = "";
    static int p2Score = 0;
    
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
        // Spawner.instance.StartMultiPlayerSpawnerCR();
    }

    void Update() {
        if (isPlaying && !GameManager.isPaused) {
            timer -= Time.deltaTime;
            if (timer < 0) {
                isPlaying = false;
                timer = 0;
                EndGame();
            }
            UpdateTimerText();
        }
    }
    //----------------------Functions-----------------------
    
    public void ButtonRestartSingle() {
        GameManager.instance.ChangeSceneSinglePlayer();
    }

    public void ButtonRestart() {
        GameManager.instance.ChangeSceneSinglePlayer();
    }

    public void ButtonToMenu() {
        GameManager.instance.ChangeSceneMenu();
    }

    public void SetupSingleplayer(float timer) {
        PlayManager.timer = timer;
        PlayManager.p1Name = "You";
        PlayManager.p2Name = "";
        PlayManager.p1Score = 0;
        PlayManager.p2Score = 0;

        this.timerText.gameObject.SetActive(true);
        this.p1Container.gameObject.SetActive(true);
        this.p2Container.gameObject.SetActive(false);

        UpdateNameText();
        UpdateScoreText();

        isPlaying = true;
        isMultiplayer = false;
        
        Spawner.instance.StartSinglePlayerSpawner(GameManager.instance.singleModeID);
    }

    public void SetupMultiplayer(string p1Name, string p2Name, float timer) {
        PlayManager.timer = timer;
        PlayManager.p1Name = p1Name;
        PlayManager.p2Name = p2Name;
        PlayManager.p1Score = 0;
        PlayManager.p2Score = 0;

        this.timerText.gameObject.SetActive(true);
        this.p1Container.gameObject.SetActive(true);
        this.p2Container.gameObject.SetActive(true);

        UpdateNameText();
        UpdateScoreText();

        isPlaying = true;
        isMultiplayer = true;
        Spawner.instance.StartMultiPlayerSpawner();
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
        string min = "" + (int)(timer / 60);
        string sec = "" + (int)(timer % 60);
        if (min.Length < 2) min = "0" + min;
        if (sec.Length < 2) sec = "0" + sec;
        timerText.text = min + ":" + sec;
    }

    public void UpdateScores(int p1Score, int p2Score) {
        PlayManager.p1Score = p1Score;
        PlayManager.p2Score = p2Score;

        UpdateScoreText();
    }

    public void UpdateScores(int id1, int score1, int id2, int score2) {
        if (id1 == Client.instance.myId) {
            UpdateScores(score1, score2);
        } else {
            UpdateScores(score2, score1);
        }
    }

    ///<summary>Trigger the end of the game</summary>
    public static void EndGame() {
        // use stopping time for now
        // Time.timeScale = 0;

        Debug.Log("PlayManager: EndGame()");
        Debug.Log("Player 1 score: " + p1Score);        
        Debug.Log("Player 2 score: " + p2Score);

        // please remove the "!" in the if statement once bug fix is complete
        if(PlayManager.instance.isMultiplayer) {
            PanelManager.StaticNext("Conclusion Panel");
            PlayManager.instance.p1ScoreEndgame.text = p1Name + " " + p1Score;
            PlayManager.instance.p2ScoreEndgame.text = p2Name + " " + p2Score;
                 
        } else {
            PanelManager.StaticNext("Single P Conclusion Panel");
            PlayManager.instance.p1ScoreEndgameSingle.text = "your score: " + p1Score;
        }
    }

    public static void AddSinglePlayerScore() {
        if (instance == null) return;

        p1Score++;
        instance.UpdateScoreText();
    }
}
