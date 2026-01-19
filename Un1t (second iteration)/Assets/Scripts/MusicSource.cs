using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicSource : MonoBehaviour
{
    private AudioSource source;
    private float baseVolume;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        baseVolume = source.volume;
    }

    private void OnEnable()
    {
        AudioMixer.Instance.GeneralVolumeChanged.AddListener(OnVolumeChanged);
        AudioMixer.Instance.MusicVolumeChanged.AddListener(OnVolumeChanged);
    }

    private void OnDisable()
    {
        AudioMixer.Instance.GeneralVolumeChanged.RemoveListener(OnVolumeChanged);
        AudioMixer.Instance.MusicVolumeChanged.RemoveListener(OnVolumeChanged);
    }

    private void Start()
    {
        OnVolumeChanged();
    }

    private void OnVolumeChanged()
    {
        source.volume = baseVolume * AudioMixer.Instance.GeneralVolume * AudioMixer.Instance.MusicVolume;
    }
}
