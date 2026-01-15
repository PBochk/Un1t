using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Slider generalSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectsSlider;
    public Slider GeneralSlider => generalSlider;
    public Slider MusicSlider => musicSlider;
    public Slider SoundEffectsSlider => soundEffectsSlider;

    private void Start()
    {
        canvas.worldCamera = Camera.main;
        canvas.enabled = false;
        generalSlider.value = AudioMixer.Instance.GeneralVolume;
        musicSlider.value = AudioMixer.Instance.MusicVolume;
        soundEffectsSlider.value = AudioMixer.Instance.SoundEffectsVolume;
    }

    private void OnEnable()
    {
        GeneralSlider.onValueChanged.AddListener(AudioMixer.Instance.OnGeneralVolumeChanged);
        MusicSlider.onValueChanged.AddListener(AudioMixer.Instance.OnMusicVolumeChanged);
        SoundEffectsSlider.onValueChanged.AddListener(AudioMixer.Instance.OnSoundEffectsVolumeChanged);
    }

    private void OnDisable()
    {
        GeneralSlider.onValueChanged.RemoveListener(AudioMixer.Instance.OnGeneralVolumeChanged);
        MusicSlider.onValueChanged.RemoveListener(AudioMixer.Instance.OnMusicVolumeChanged);
        SoundEffectsSlider.onValueChanged.RemoveListener(AudioMixer.Instance.OnSoundEffectsVolumeChanged);
    }
}
