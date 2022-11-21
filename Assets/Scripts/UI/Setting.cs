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

    void Start() {
        if (!isSet) {
            musicVolumeWhole = (int)musicSlider.value;
            sfxVolumeWhole = (int)sfxSlider.value;
            isSet = true;
        } else {
            musicSlider.value = musicVolumeWhole;
            sfxSlider.value = sfxVolumeWhole;
        }
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
}
