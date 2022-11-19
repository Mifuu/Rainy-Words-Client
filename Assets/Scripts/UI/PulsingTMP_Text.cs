using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class PulsingTMP_Text : MonoBehaviour
{
    TMP_Text text;
    
    [Header("Pulse Settings")]
    public float pulsePeriod = 0.6f;
    public Color secondaryColor = Color.clear;

    float pulseValue = 0;
    Color initialColor;
    float nextPulseTime = 0;

    void Awake() {
        text = GetComponent<TMP_Text>();
    }

    void Start() {
        initialColor = text.color;
    }

    void Update() {
        pulseValue = Mathf.InverseLerp(-1, 1, Mathf.Sin(Time.time * 2 * Mathf.PI / pulsePeriod));
        SetPulseColor(pulseValue);
    }

    void SetPulseColor(float value) {
        text.color = Color.Lerp(initialColor, secondaryColor, value);
    }
}
