using UnityEngine;
using UnityEngine.UI;

public class AudioMixerUI : MonoBehaviour
{
    [SerializeField] private Slider generalSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectsSlider;
    [SerializeField] private Canvas canvas;
    public Slider GeneralSlider => generalSlider;
    public Slider MusicSlider => musicSlider;
    public Slider SoundEffectsSlider => soundEffectsSlider;
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        GeneralSlider.onValueChanged.AddListener(AudioMixer.Instance.OnGeneralVolumeChanged);
        MusicSlider.onValueChanged.AddListener(AudioMixer.Instance.OnMusicVolumeChanged);
        SoundEffectsSlider.onValueChanged.AddListener(AudioMixer.Instance.OnSoundEffectsVolumeChanged);
        canvas.gameObject.SetActive(false);
    }
}
