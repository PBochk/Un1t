using UnityEngine;
using UnityEngine.UI;

public class AudioMixerUI : MonoBehaviour
{
    [SerializeField] private Slider generalSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectsSlider;
    public Slider GeneralSlider => generalSlider;
    public Slider MusicSlider => musicSlider;
    public Slider SoundEffectsSlider => soundEffectsSlider;
    private void Awake()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
