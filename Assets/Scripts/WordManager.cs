using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    public GameObject lowerBound;

    void FixedUpdate() {
        foreach (Word w in Word.words) {
            if (w.transform.position.y > lowerBound.transform.position.z) continue;
            w.Remove();
            Time.timeScale = 0;
        }
    }
}
