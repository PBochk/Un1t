using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicSource : MonoBehaviour
{
    private AudioMixer mixer;
    private AudioSource source;
    private float baseVolume;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        baseVolume = source.volume;
    }
    private void Start()
    {
        mixer = AudioMixer.Instance;
        mixer.GeneralVolumeChanged.AddListener(OnVolumeChanged);
        mixer.MusicVolumeChanged.AddListener(OnVolumeChanged);
        OnVolumeChanged();
    }

    private void OnVolumeChanged()
    {
        source.volume = baseVolume * mixer.GeneralVolume * mixer.MusicVolume;
    }
}
