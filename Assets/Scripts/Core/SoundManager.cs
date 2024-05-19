using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // only one static is create no matter what (because there's only one SoundManager)
    public static SoundManager instance { get; private set; }
    private AudioSource effectSource;
    private AudioSource musicSource;


    private void Awake()
    {
        if (instance != null && instance != this) 
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        effectSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip _sound)
    {
        effectSource.PlayOneShot(_sound);
    }

    public void ChangeEffectVolume(float _change)
    {
        ChangeSoundVolume(_change, "effectVolume", effectSource);

    }

    public void ChangeMusicVolume(float _change)
    {
        ChangeSoundVolume(_change, "musicVolume", musicSource);
    }

    private void ChangeSoundVolume(float _change, string volumeName, AudioSource source)
    {
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += _change;

        if (currentVolume > 1)
        {
            currentVolume = 0;
        }
        else if (currentVolume < 0)
        {
            currentVolume = 1;
        }

        source.volume = currentVolume;

        PlayerPrefs.SetFloat(volumeName, currentVolume);
    }
}
