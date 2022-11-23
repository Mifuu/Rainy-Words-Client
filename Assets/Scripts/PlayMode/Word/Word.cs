using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
    Falling word component
*/
[RequireComponent(typeof(TMP_Text))]
public class Word : MonoBehaviour
{
    public string text = "default";
    
    [Header("Movement")]
    public float velocityY = -20;

    // references
    public TMP_Text tmp;
    
    //----------------------Functions-----------------------
    void Awake() {
        // add self to words list
        WordManager.words.Add(this);
    }

    void FixedUpdate() {
        // apply velocity
        if (!GameManager.isPaused) {
            Vector3 pos = transform.position;
            pos.y += velocityY * Time.fixedDeltaTime;
            transform.position = pos;
        }
    }

    public void Remove() {
        Destroy(gameObject);
        // StartCoroutine(RemoveCR());
    }

    IEnumerator RemoveCR() {
        // add a little delay
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);
    }

    public void SetText(string _text) {
        text = _text;
        tmp.text = _text;
    }

    //----------------------Utility-----------------------
    // return hex value of a color
    public static string GetHex(Color col) {
        return ColorUtility.ToHtmlStringRGBA(col);
    }

    //----------------------Events-----------------------
    void OnDestroy() {
        WordManager.words.Remove(this);
    }
}
