using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    //[SerializeField] private CanvasGroup sliders;
    [SerializeField] private Slider generalSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectsSlider;
    public Slider GeneralSlider => generalSlider;
    public Slider MusicSlider => musicSlider;
    public Slider SoundEffectsSlider => soundEffectsSlider;

    private void Start()
    {
        canvas.worldCamera = Camera.main;
        GeneralSlider.onValueChanged.AddListener(AudioMixer.Instance.OnGeneralVolumeChanged);
        MusicSlider.onValueChanged.AddListener(AudioMixer.Instance.OnMusicVolumeChanged);
        SoundEffectsSlider.onValueChanged.AddListener(AudioMixer.Instance.OnSoundEffectsVolumeChanged);
        canvas.enabled = false;
    }
}
