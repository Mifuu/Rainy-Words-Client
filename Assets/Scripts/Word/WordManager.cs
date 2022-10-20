using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manage all words
public class WordManager : MonoBehaviour
{
    public static WordManager instance;
    public Transform lowerBound;   // use to set lowerbound of the game

    //----------------------Start-----------------------
    void Awake() {
        // singleton
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    void FixedUpdate() {
        foreach (Word w in Word.words) {
            // if each word is above lowerbound, continue
            if (w.transform.position.y > lowerBound.position.y) continue;

            // if any word go passed the lowerbound, remove that word and end the game
            w.Remove();
            GameManager.EndGame();
            break;
        }
    }
}
