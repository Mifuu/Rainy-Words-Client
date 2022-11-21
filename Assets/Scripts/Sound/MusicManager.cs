using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    // singleton
    public static MusicManager instance;

    static public float musicVolume = 1;

    public Sound theme1;

    bool bossMode = false;

    void Awake() {
        // singleton
        if (instance == null) instance = this;
        else Destroy(this);

        Setup(theme1);
    }

    void Start() {
        Play(theme1);
        SetMusicVolume(musicVolume);
    }

    void Update() {
        
    }

    public void Setup(Sound sound) {
        sound.source = gameObject.AddComponent<AudioSource>();
        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
        sound.source.loop = sound.loop;
    }

    void Play(Sound sound) {
        sound.source.Play();
    }

    public void SetMusicVolume(float vol) {
        musicVolume = vol;
        theme1.source.volume = theme1.volume * musicVolume;
    }
}

[System.Serializable]
public class Sound {
    public string name;
    public AudioClip clip;

    [Range(0f,2.5f)]
    public float volume = 1;
    [Range(0.1f,3f)]
    public float pitch = 1;
    public bool loop = false;

    [Header("Don't Fill")]
    public AudioSource source;
}
