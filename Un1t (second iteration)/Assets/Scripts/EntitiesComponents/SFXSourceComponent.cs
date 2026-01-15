using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class SFXSourceComponent : MonoBehaviour
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
        AudioMixer.Instance.SoundEffectsVolumeChanged.AddListener(OnVolumeChanged);
    }

    private void OnDisable()
    {
        AudioMixer.Instance.GeneralVolumeChanged.RemoveListener(OnVolumeChanged);
        AudioMixer.Instance.SoundEffectsVolumeChanged.RemoveListener(OnVolumeChanged);
    }


    private void Start()
    {
        OnVolumeChanged();
    }

    private void OnVolumeChanged()
    {
        source.volume = baseVolume * AudioMixer.Instance.GeneralVolume * AudioMixer.Instance.SoundEffectsVolume;
    }
}
