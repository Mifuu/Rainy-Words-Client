using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// set word list and get random word
public class WordList
{
    public static List<Word> words = new List<Word> ();     // tracking all word component that currently exists
    public static Dictionary<string, string[]> wordLists = new Dictionary<string, string[]> ();
    private static char[] delimiterChars = {'\n', ' '};
    
    /// <summary> get random word from a specefic group </summary>
    public static string GetRandomWord(string listname) {
        // check if key exist
        if (!wordLists.ContainsKey(listname)) {
            // no key
            Debug.Log("WordList: ERROR, cannot find list name " + listname);
            return null;
        }

        // key exist, get random word
        string[] list = wordLists[listname];
        int rand = Random.Range(0, list.Length);
        return list[rand];
    }

    /// <summary> add new list of word </summary>
    public static void AddListResource(string filename, string listname) {
        // https://forum.unity.com/threads/get-a-random-word-from-the-dictionary.383833/
        TextAsset textAsset = (TextAsset)Resources.Load(filename);
        string[] newList = textAsset.text.Split(delimiterChars);
        wordLists.Add(listname, newList);
    }
}
