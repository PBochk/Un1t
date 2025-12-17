using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerUI : MonoBehaviour
{
    [SerializeField] private Slider generalSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectsSlider;
    private MainUI mainUI;
    private AudioMixer audioMixer;
    public Slider GeneralSlider => generalSlider;
    public Slider MusicSlider => musicSlider;
    public Slider SoundEffectsSlider => soundEffectsSlider;
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        mainUI = GetComponentInParent<MainUI>();
        audioMixer = mainUI.AudioMixer;
        GeneralSlider.onValueChanged.AddListener(audioMixer.OnGeneralVolumeChanged);
        MusicSlider.onValueChanged.AddListener(audioMixer.OnMusicVolumeChanged);
        SoundEffectsSlider.onValueChanged.AddListener(audioMixer.OnSoundEffectsVolumeChanged);
    }
}
