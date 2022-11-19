using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageSpritesheetPlayer : MonoBehaviour
{
    [Range(1, 100)]
    public float fps = 5;
    public Sprite[] sprites;

    float nextFrameTime = 0;
    int index = 0;

    Image image;
    
    void Awake() {
        image = GetComponent<Image> ();
    }

    void Update() {
        float period = 1/fps;

        if (Time.time > nextFrameTime) {
            nextFrameTime = (Mathf.Floor(Time.time / period) + 1) * period;

            index += 1;
            index %= sprites.Length;

            image.sprite = sprites[index];
        }
    }
}
