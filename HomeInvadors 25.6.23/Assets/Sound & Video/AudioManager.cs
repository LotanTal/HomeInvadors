using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sounds[] music, sfx, spatialAudio, score;
    public AudioSource musicSource, sfxSource, scoreSFXSource;
    public AudioSource[] spatialAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("main");
    }

    public void PlayMusic(string name)
    {
        Sounds s = Array.Find(music, x => x.name == name);

        if (s == null)
        {
            Debug.Log("No Sound!!");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySpatialAudio(string name, int index)
    {
        Sounds s = Array.Find(spatialAudio, x => x.name == name);

        if (s == null)
        {
            Debug.Log("No Sound!!");
        }
        else if (index < 0 || index >= spatialAudioSource.Length)
        {
            Debug.Log("Invalid AudioSource index!");
        }
        else
        {
            spatialAudioSource[index].clip = s.clip;
            spatialAudioSource[index].Play();
        }
    }

    public void playSFX(string name)
    {
        Sounds s = Array.Find(sfx, x => x.name == name);

        if (s == null)
        {
            Debug.Log("No sfx!!");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void playSFXscore(string name)
    {
        Sounds s = Array.Find(score, x => x.name == name);

        if (s == null)
        {
            Debug.Log("No sfx!!");
        }
        else
        {
            scoreSFXSource.PlayOneShot(s.clip);
        }

    }
}
