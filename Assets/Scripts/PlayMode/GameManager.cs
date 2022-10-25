using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Manage the state of game inside Game scene
public class GameManager : MonoBehaviour
{
    public static GameManager instance;     // for singleton access

    // variables
    public static int score = 0;

    // reference
    public TMP_Text scoreText;

    //----------------------Start and Update-----------------------
    void Awake() {
        // singleton: 1 object at a time contain this code
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        // add wordlist
        WordList.AddListResource("wordlist10000", "wordlist10000");
    }

    void Update() {
        // update score text
        if (scoreText != null) scoreText.text = "score: " + score;
    }

    //----------------------Functions-----------------------
    ///<summary>Trigger the end of the game</summary>
    public static void EndGame() {
        // use stopping time for now
        Time.timeScale = 0;
    }
}
