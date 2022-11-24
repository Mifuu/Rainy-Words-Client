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
            if (GameManager.nextSceneMode != GameManager.NextSceneMode.Single) {
                // multiplayer case
                ConnectionManager.DeliverMsg("wordExpire", w.text);
            } else {
                // singleplayer case: gameover
                // stop game
                GameManager.instance.Pause(true);
                SFXManager._PlaySFX("Gameover1", PlayManager.instance.gameObject);
                StartCoroutine(DelayGameover());
            }
            w.Remove();

            // TODO: End game for single player
            break;
        }
    }

    IEnumerator DelayGameover() {
        yield return new WaitForSeconds(1.75f);
        // PanelManager.StaticNext("Single P Conclusion Panel");
        PlayManager.EndGame();
    }

    //----------------------Check-----------------------
    // after [Enter] check if any word exactly matches
    public static bool CheckWord(string word, bool removeMatchingWord) {
        int count = 0;

        foreach (Word w in words) {
            if (w.text.Equals(word)) {
                if(removeMatchingWord) w.Remove();
                count++;
                if (w.checkWordParticle != null) Instantiate(w.checkWordParticle, w.transform.position, Quaternion.identity);
                // GameManager.score++;
            }
        }

        if (count > 0) return true;
        return false;
    }

    // change color of typed word that patially match what is being type
    public static bool CheckTyping(string word, Color col) {
        if (word.Equals("")) return false;

        int count = 0;
        foreach (Word w in words) {
            if (word.Length > w.text.Length) {
                w.tmp.text = w.text;
                continue;
            }

            string text = w.text.Substring(0, word.Length);
            if (text.Equals(word)) {
                w.tmp.text = "<color=#" + Word.GetHex(col) + ">" + text + "<color=#" + Word.GetHex(w.tmp.color) + ">" + w.text.Substring(word.Length, w.text.Length - word.Length);
                if (w.checkTypeParticle != null) Instantiate(w.checkTypeParticle, w.transform.position, Quaternion.identity);
                count++;
            } else {
                w.tmp.text = w.text;
            }
        }

        if (count > 0) return true;
        return false;
    }

    public static bool CheckWordNetcentric(string word, bool removeMatchingWord) {
        int count = 0;

        foreach (Word w in words) {
            if (w.text.Equals(word)) {
                if(removeMatchingWord) w.Remove();
                count++;
                if (w.checkWordParticle != null) Instantiate(w.checkWordParticle, w.transform.position, Quaternion.identity);
                // GameManager.score++;
            }
        }

        if (count > 0) return true;
        return false;
    }

    public static bool CheckTypingNetcentric(string word, Color col) {
        if (word.Equals("")) return false;

        int count = 0;
        foreach (Word w in words) {
            if (word.Length > w.text.Length) {
                w.tmp.text = w.nText;
                continue;
            }

            string text = w.text.Substring(0, word.Length);
            if (w.text.Equals(word)) {
                // directly equal
                w.tmp.text = "<color=#" + Word.GetHex(col) + ">" + w.nText + "<color=#" + Word.GetHex(w.tmp.color) + ">";
            } if (text.Equals(word)) {
                string[] textWord = w.text.Split(' ');
                
                int matchWordCount = 0;
                int currentCharNum = 0;
                foreach (string _w in textWord) {
                    currentCharNum += _w.Length;
                    if (text.Length >= currentCharNum) {
                        matchWordCount++;
                        currentCharNum++; // from space
                    } else {
                        break;
                    }
                }

                string tmpText1 = w.nText.Substring(0, matchWordCount);
                string tmpText2 = w.nText.Substring(matchWordCount);

                w.tmp.text = "<color=#" + Word.GetHex(col) + ">" + tmpText1 + "<color=#" + Word.GetHex(w.tmp.color) + ">" + tmpText2;
                if (w.checkTypeParticle != null) Instantiate(w.checkTypeParticle, w.transform.position, Quaternion.identity);
                count++;
            } else {
                w.tmp.text = w.nText;
            }
        }

        if (count > 0) return true;
        return false;
    }
}
