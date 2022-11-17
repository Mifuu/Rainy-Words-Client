using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manage all words
public class WordManager : MonoBehaviour
{
    public static List<Word> words = new List<Word> ();     // tracking all word component that currently exists

    public static WordManager instance;                     // singleton
    public Transform lowerBound;                            // use to set lowerbound of the game to erase the word that pass this bound

    //----------------------Start-----------------------
    void Awake() {
        // singleton
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    void FixedUpdate() {
        CheckWordPassLowerBound();
    }

    // if word pass lower bound, tell the server that the word has expire
    void CheckWordPassLowerBound() {
        foreach (Word w in words) {
            // if each word is above lowerbound, continue
            if (w.transform.position.y > lowerBound.position.y) continue;

            // if any word go passed the lowerbound, remove that word and end the game
            ConnectionManager.DeliverMsg("wordExpire", w.text);
            w.Remove();

            // TODO: End game for single player
            break;
        }
    }

    //----------------------Check-----------------------
    // after [Enter] check if any word exactly matches
    public static bool CheckWord(string word, bool removeMatchingWord) {
        int count = 0;

        foreach (Word w in words) {
            if (w.text.Equals(word)) {
                if(removeMatchingWord) w.Remove();
                count++;
                // GameManager.score++;
            }
        }

        if (count > 0) return true;
        return false;
    }

    // change color of typed word that patially match what is being type
    public static void CheckTyping(string word, Color col) {
        foreach (Word w in words) {
            if (word.Length > w.text.Length) {
                w.tmp.text = w.text;
                continue;
            }

            string text = w.text.Substring(0, word.Length);
            if (text.Equals(word)) {
                w.tmp.text = "<color=#" + Word.GetHex(col) + ">" + text + "<color=#" + Word.GetHex(w.tmp.color) + ">" + w.text.Substring(word.Length, w.text.Length - word.Length);
            } else {
                w.tmp.text = w.text;
            }
        }
    }
}
