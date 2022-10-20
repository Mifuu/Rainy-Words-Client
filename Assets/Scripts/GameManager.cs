using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public int score = 0;

    // reference
    public TMP_Text scoreText;
    public Spawner1 spawner;

    //----------------------Functions-----------------------

    void Awake() {
        if (gameManager == null) gameManager = this;
    }

    void Update() {
        if (scoreText != null) scoreText.text = "score: " + score;
    }
}
