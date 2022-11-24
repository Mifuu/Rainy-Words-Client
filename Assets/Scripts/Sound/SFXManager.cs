using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    // singleton
    public static SFXManager instance;

    static public float sfxVolume = 1;
    static float sfxVolumeFactor = .15f;
    public Sound[] sounds;
    private List<AudioSource> audioSources; //keep track for setting volume

    public void Awake() {
        if (SFXManager.instance == null) instance = this;
        else Destroy(this);

        audioSources = new List<AudioSource>();
    }

    public void Start() {
        SetSFXVolume(sfxVolume);
    }

    public void PlaySFX(string name, GameObject target) {
        // check empty name
        if (name.Equals("")) return;

        // find sound
        Sound s = Array.Find(sounds, sound => sound.name.Equals(name));
        if (s == null) {
            Debug.Log("SFXManager: ERROR, SFX with the name \"" + name + "\" cannot be found");
            return;
        }

        // ensure source
        AudioSource source = target.GetComponent<AudioSource>();
        if (source == null) {
            source = target.AddComponent<AudioSource>();
            source.volume = sfxVolume;
            audioSources.Add(source);
            source.spatialBlend = .4f;
        }

        if (source.enabled) source.PlayOneShot(s.clip, s.volume * sfxVolume * sfxVolumeFactor);
    }

    public static void _PlaySFX(string name, GameObject target) {
        if (instance == null) {
            Debug.Log("SFXManager: ERROR, no instance found!");
            return;
        }
        instance.PlaySFX(name, target);
    }

    public void SetSFXVolume(float vol) {
        sfxVolume = vol;
        foreach (AudioSource a in audioSources) {
            if (a == null) {
                audioSources.Remove(a);
                continue;
            } else {
                a.volume = sfxVolume * sfxVolumeFactor;
            }
        }
    }
}
