using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AudioMixer : MonoBehaviour
{
    [SerializeField] private AudioMixerUI audioMixerUI;
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
        audioMixerUI.GeneralSlider.onValueChanged.AddListener(OnGeneralVolumeChanged);
        audioMixerUI.MusicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        audioMixerUI.SoundEffectsSlider.onValueChanged.AddListener(OnSoundEffectsVolumeChanged);
    }

    private void OnGeneralVolumeChanged(float value)
    {
        GeneralVolume = value;
        GeneralVolumeChanged?.Invoke();
    }

    private void OnMusicVolumeChanged(float value)
    {
        MusicVolume = value;
        MusicVolumeChanged?.Invoke();
    }

    private void OnSoundEffectsVolumeChanged(float value)
    {
        SoundEffectsVolume = value;
        SoundEffectsVolumeChanged?.Invoke();
    }
}
