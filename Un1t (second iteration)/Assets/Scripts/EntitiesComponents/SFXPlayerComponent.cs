using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class SFXPlayerComponent : MonoBehaviour
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
        mixer.SoundEffectsVolumeChanged.AddListener(OnVolumeChanged);
        Debug.Log($"Base: {baseVolume}, General: {mixer.GeneralVolume}, SFX:  {mixer.SoundEffectsVolume}");
    }

    private void OnVolumeChanged()
    {
        Debug.Log($"Base: {baseVolume}, General: {mixer.GeneralVolume}, SFX:  {mixer.SoundEffectsVolume}");
        source.volume = baseVolume * mixer.GeneralVolume * mixer.SoundEffectsVolume;
    }
}
