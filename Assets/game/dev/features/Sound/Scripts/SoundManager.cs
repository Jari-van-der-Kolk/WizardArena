using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static List<SoundEffect> sounds;
    public static event Action<float> OnVolumeChanged;

    private static SoundManager singleton;
    public static SoundManager Singleton
    {
        get
        {
            return singleton;
        }

        private set
        {
            if (singleton)
            {
                Destroy(value);
                Debug.LogError("We have more than one SoundManager!!!");
                return;
            }

        }
    }

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this);
        }

        //makes AudioSource components on this gameobject and initializes their settings from the ScriptableObject
        foreach (SoundEffect s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Play(SoundEffect soundEffect)
    {
        if (soundEffect.clip == null)
        {
            Debug.LogError(soundEffect.name + " Does not contain an audioclip");
            return;
        }
        soundEffect.source.PlayOneShot(soundEffect.source.clip);
    }

    public static void Subscribe(SoundEffect s)
    {
        if (sounds == null)
        {
            sounds = new List<SoundEffect>();
        }
        sounds.Add(s);
    }

    // Method to notify observers about volume changes
    private void NotifyVolumeChanged(float newVolume)
    {
        OnVolumeChanged?.Invoke(newVolume);
    }

    // Method to set volume for all audio sources
    public void SetVolume(Slider slider)
    {
        foreach (SoundEffect soundEffect in sounds)
        {
            soundEffect.source.volume = slider.value;
        }

        NotifyVolumeChanged(slider.value);
    }

    public void LoadSlider(Slider slider)
    {
        slider.value = PlayerPrefs.GetFloat("Volume", .25f);
        SetVolume(slider);
    }
}