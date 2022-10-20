using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class Word : MonoBehaviour
{
    public static List<Word> words = new List<Word> ();
    static string[] dict;
    static int count = 0;
    public enum Type {Random, Sequence}
    public static Type type = Type.Random;
    public static char[] delimiterChars = {'\n', ' '};

    string text;
    [Header("Movement")]
    public float velocityY = -20;

    // references
    public TMP_Text tmp;
    
    //----------------------Functions-----------------------
    void Awake() {
        // if static dictionary doesn't exist, get one
        if (dict == null) SetDict();

        // add self to words
        words.Add(this);
    }

    void Start() {
        if (type == Type.Random) {
            RandomizeText();
        } else if (type == Type.Sequence) {
            SequentialText();
        }
    }

    void FixedUpdate() {
        // apply velocity
        Vector3 pos = transform.position;
        pos.y += velocityY * Time.fixedDeltaTime;
        transform.position = pos;
    }

    //----------------------Word Functions-----------------------
    void RandomizeText() {
        // check
        if (dict == null || dict.Length == 0) SetDict();

        // random text
        int rand = Random.Range(0, dict.Length);
        text = dict[rand];

        // apply text to TMP_Text
        tmp.text = text;
    }

    void SequentialText() {
        // check
        if (dict == null || dict.Length == 0) SetDict();

        // next text
        text = dict[count];
        count++;

        // apply text to TMP_Text
        tmp.text = text;
    }

    static void SetDict() {
        SetDict("wordlist10000");
    }

    public static void SetDict(string filename) {
        // https://forum.unity.com/threads/get-a-random-word-from-the-dictionary.383833/
        TextAsset textAsset = (TextAsset)Resources.Load(filename);
        dict = textAsset.text.Split(delimiterChars);
    }

    public void Remove() {
        StartCoroutine(RemoveCR());
    }

    IEnumerator RemoveCR() {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    static string GetHex(Color col) {
        return ColorUtility.ToHtmlStringRGBA(col);
    }

    //----------------------Events-----------------------
    public static bool CheckWord(string word) {
        int count = 0;

        foreach (Word w in words) {
            if (w.text.Equals(word)) {
                w.Remove();
                count++;
                GameManager.gameManager.score++;
            }
        }

        if (count > 0) return true;
        return false;
    }

    public static void CheckTyping(string word, Color col) {
        foreach (Word w in words) {
            if (word.Length > w.text.Length) {
                w.tmp.text = w.text;
                continue;
            }

            string text = w.text.Substring(0, word.Length);
            if (text.Equals(word)) {
                w.tmp.text = "<color=#" + GetHex(col) + ">" + text + "<color=#" + GetHex(w.tmp.color) + ">" + w.text.Substring(word.Length, w.text.Length - word.Length);
            } else {
                w.tmp.text = w.text;
            }
        }
    }

    //----------------------Events-----------------------
    void OnDestroy() {
        words.Remove(this);
    }
}
