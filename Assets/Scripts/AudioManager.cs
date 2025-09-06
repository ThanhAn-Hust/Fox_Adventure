using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }

    public AudioClip sfx_mushroom; // sound effect cho cherry, co the them sfx khac neu muon
    public AudioClip ambient_music;
    public GameObject soundObject;

    public void PlaySFX(string sfxName)
    {
        switch(sfxName)
        {
            case "mushroom":
                SoundObjectCreation(sfx_mushroom);
                break;
            default:
                break;
        }
    }

    void SoundObjectCreation(AudioClip clip)
    {
        GameObject newObject = Instantiate(soundObject, transform);
        newObject.GetComponent<AudioSource>().clip = clip;
        newObject.GetComponent<AudioSource>().Play();

    }

    public void PlayMusic(string musicName)
    {
        switch (musicName)
        {
            case "ambient":
                MusicObjectCreation(ambient_music);
                break;
            default:
                break;
        }
    }

    void MusicObjectCreation(AudioClip clip)
    {
        GameObject newObject = Instantiate(soundObject, transform);
        newObject.GetComponent<AudioSource>().clip = clip;
        newObject.GetComponent<AudioSource>().loop = true;
        newObject.GetComponent<AudioSource>().Play();

    }
}
