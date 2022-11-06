using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//https://answers.unity.com/questions/940456/how-to-change-text-color-on-hover-in-new-gui.html

[RequireComponent (typeof(Button))]
public class FancyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
  
    TMP_Text txt;
    Color baseColor;
    Button btn;
    RectTransform rect;
    bool interactableDelay;

    [Header("Color Event")]
    public Color normalColor = Color.white;
    public Color highlightedColor = new Color(0,0,0,1);
    public Color pressedColor = new Color(0,0,0,1);
    public Color disabledColor = new Color(1,1,1,0.5f);

    // LeanTween
    [Header("Tween Event")]
    public bool doTween = true;
    public float tweenTime = 0.75f;
    public float enterScaleTween = 1.17f;
    public float downScaleTween = 1.06f;
    public float upScaleTween = 1.08f;
    public float exitScaleTween = 1.04f;
    Vector3 initialLocalScale;
 
    void Start ()
    {
        txt = GetComponentInChildren<TMP_Text>();
        baseColor = txt.color;
        btn = gameObject.GetComponent<Button> ();
        interactableDelay = btn.interactable;

        // LeanTween
        initialLocalScale = transform.localScale;
    }
 
    void Update ()
    {
        // update for clicking case
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) ||
        Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2)) {
            interactableDelay = !btn.interactable;
        }

        // update for change panel
        if (PanelManager.instance != null && PanelManager.instance.frameSincePanelUpdated < 5) {
            interactableDelay = !btn.interactable;
        }

        if (btn.interactable != interactableDelay) {
            if (btn.interactable) {
                if (EventSystem.current.currentSelectedGameObject == gameObject) {
                    txt.color = baseColor * highlightedColor * btn.colors.colorMultiplier;
                } else {
                    txt.color = baseColor * normalColor * btn.colors.colorMultiplier;
                }
            } else {
                txt.color = baseColor * disabledColor * btn.colors.colorMultiplier;
            }
        }
        interactableDelay = btn.interactable;
    }
 
    public void OnPointerEnter (PointerEventData eventData)
    {
        if (btn.interactable) {
            txt.color = baseColor * highlightedColor * btn.colors.colorMultiplier;
            Tween(enterScaleTween);
        } else {
            txt.color = baseColor * disabledColor * btn.colors.colorMultiplier;
        }
    }
 
    public void OnPointerDown (PointerEventData eventData)
    {
        if (btn.interactable) {
            txt.color = baseColor * pressedColor * btn.colors.colorMultiplier;
            Tween(downScaleTween);
        } else {
            txt.color = baseColor * disabledColor * btn.colors.colorMultiplier;
        }
    }
 
    public void OnPointerUp (PointerEventData eventData)
    {
        if (btn.interactable) {
            txt.color = baseColor * highlightedColor * btn.colors.colorMultiplier;
            Tween(upScaleTween);
        } else {
            txt.color = baseColor * disabledColor * btn.colors.colorMultiplier;
        }
    }
 
    public void OnPointerExit (PointerEventData eventData)
    {
        if (btn.interactable) {
            if (EventSystem.current.currentSelectedGameObject == gameObject) {
                txt.color = baseColor * highlightedColor * btn.colors.colorMultiplier;
            } else {
                txt.color = baseColor * normalColor * btn.colors.colorMultiplier;
            }
            Tween(exitScaleTween);
        } else {
            txt.color = baseColor * disabledColor * btn.colors.colorMultiplier;
        }
    }
 
    public void Tween(float scale) {
        LeanTween.cancel(gameObject);

        if (!doTween) return;

        transform.localScale = initialLocalScale;

        LeanTween.scale(gameObject, Vector3.one * scale, tweenTime)
            .setEasePunch();

    }

    void OnEnabled() {
        txt.color = baseColor * normalColor * btn.colors.colorMultiplier;
    }

    void OnDisabled() {
        txt.color = baseColor * normalColor * btn.colors.colorMultiplier;
    }
}
