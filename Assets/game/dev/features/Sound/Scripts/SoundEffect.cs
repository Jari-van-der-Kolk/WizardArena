using UnityEngine;

[CreateAssetMenu(fileName = "SoundEffect")]
public class SoundEffect : ScriptableObject, IVolumeObserver
{
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(1f, 3f)] public float pitch = 1f;
    public bool loop;

    [HideInInspector]
    public AudioSource source;


    private void OnEnable()
    {
        SoundManager.Subscribe(this);
        SoundManager.OnVolumeChanged += OnVolumeChanged;
    }
    public void OnVolumeChanged(float newVolume)
    {
        PlayerPrefs.SetFloat("Volume", newVolume); 
    }

}
