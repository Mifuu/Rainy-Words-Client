using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public static bool isSet = false;
    public static int musicVolumeWhole = 0;
    public static int sfxVolumeWhole = 0;

    public Button musicPlusButton;
    public Button musicMinusButton;
    public Button sfxPlusButton;
    public Button sfxMinusButton;
    public Slider musicSlider;
    public Slider sfxSlider;

    UnityEngine.Rendering.Universal.ColorAdjustments colAdj;

    void Start() {
        if (!isSet) {
            musicVolumeWhole = (int)musicSlider.value;
            sfxVolumeWhole = (int)sfxSlider.value;
            isSet = true;
        } else {
            musicSlider.value = musicVolumeWhole;
            sfxSlider.value = sfxVolumeWhole;
        }

        // get volume
        // https://forum.unity.com/threads/urp-volume-cs-how-to-access-the-override-settings-at-runtime-via-script.813093/
        UnityEngine.Rendering.VolumeProfile volumeProfile = GameManager.instance?.volumeProfile;
        if (volumeProfile == null) {
            Debug.Log("Setting: ERROR, can't get volume!");
        } else {
            if (!volumeProfile.TryGet(out colAdj)) throw new System.NullReferenceException(nameof(colAdj));
        }
    }

    void OnDisable() {
        this.enabled = true;
    }

    void OnApplicationQuite() {
        if (colAdj == null) return;
        colAdj.hueShift.value = 0;
    }

    public void ButtonMusicPlus() {
        musicSlider.value++;
    }

    public void ButtonMusicMinus() {
        musicSlider.value--;
    }

    public void ButtonSFXPlus() {
        sfxSlider.value++;
    }

    public void ButtonSFXMinus() {
        sfxSlider.value--;
    }

    public void SliderMusicChanged() {
        if (MusicManager.instance != null) {
            float value = (musicSlider.value / (musicSlider.maxValue / 2));
            MusicManager.instance.SetMusicVolume(value);
            musicVolumeWhole = (int)musicSlider.value;
        }

        SFXManager._PlaySFX("Click3", gameObject);
    }

    public void SliderSFXChanged() {
        if (SFXManager.instance != null) {
            float value = (sfxSlider.value / (sfxSlider.maxValue / 2));
            SFXManager.instance.SetSFXVolume(value);
            sfxVolumeWhole = (int)sfxSlider.value;
        }

        SFXManager._PlaySFX("Click3", gameObject);
    }

    public void ButtonScrollThemeColor() {
        if (colAdj == null) return;
        float x = colAdj.hueShift.value;
        x = (((x + 180) + 60) % 360) - 180;
        colAdj.hueShift.value = x;
    }
}
