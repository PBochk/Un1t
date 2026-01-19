using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AudioMixer : MonoBehaviour
{
    public static AudioMixer Instance { get; private set; }
    public float GeneralVolume { get; private set; } = 1.0f;
    public float MusicVolume { get; private set; } = 1.0f;
    public float SoundEffectsVolume { get; private set; } = 1.0f;
    public UnityEvent GeneralVolumeChanged;
    public UnityEvent MusicVolumeChanged;
    public UnityEvent SoundEffectsVolumeChanged;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnGeneralVolumeChanged(float value)
    {
        GeneralVolume = value;
        GeneralVolumeChanged?.Invoke();
    }

    public void OnMusicVolumeChanged(float value)
    {
        MusicVolume = value;
        MusicVolumeChanged?.Invoke();
    }

    public void OnSoundEffectsVolumeChanged(float value)
    {
        SoundEffectsVolume = value;
        SoundEffectsVolumeChanged?.Invoke();
    }
}
