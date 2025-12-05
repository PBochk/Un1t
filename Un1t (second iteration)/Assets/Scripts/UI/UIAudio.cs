using System;
using UnityEngine;

public class UIAudio : MonoBehaviour
{
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip buttonClick;

    public void PlayButtonClickSound()
    {
        soundSource.PlayOneShot(buttonClick);
    }
}