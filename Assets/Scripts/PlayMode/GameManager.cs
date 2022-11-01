using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Manage the state of game inside Game scene
public class GameManager : MonoBehaviour
{
    public static GameManager instance;     // for singleton access

    // variables
    public static int score = 0;
    public static int p1Score = 0;
    public static int p2Score = 0;

    // reference
    public TMP_Text scoreText;
    public TMP_Text scoreText1;
    public TMP_Text scoreText2;

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

    void Update() {
        // update score text
        // if (scoreText != null) scoreText.text = "score: " + score;

        // update player1 and player2 scores
        if (scoreText1 != null) scoreText1.text = "player1: " + p1Score.ToString();
        if (scoreText2 != null) scoreText2.text = "player2: " + p2Score.ToString();
    }

    //----------------------Functions-----------------------
    public static void updateScores(int p1, int p2) {
        p1Score = p1;
        p2Score = p2;
    }

    ///<summary>Trigger the end of the game</summary>
    public static void EndGame() {
        // use stopping time for now
        Time.timeScale = 0;
    }
}
