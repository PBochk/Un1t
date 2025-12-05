using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerMeleeWeaponAudio : MonoBehaviour
{
    [SerializeField] private AudioClip StoneHitSound;
    [SerializeField] private AudioClip SlimeHitSound;
    [SerializeField] private AudioClip GlitchHitSound;
    private AudioSource audioSource;
    private MeleeWeaponController controller;
    private Dictionary<HitableEntityType, AudioClip> entityTypeToSound;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<MeleeWeaponController>();
        controller.EntityHit.AddListener(PlayHitSound);
        // TODO: import serializable dictionary asset already
        entityTypeToSound = new()
        {
            {HitableEntityType.Rock, StoneHitSound},
            {HitableEntityType.Slime, SlimeHitSound},
            {HitableEntityType.Glitch, GlitchHitSound},
        };
    }

    public void PlayHitSound(HitableEntityType type)
    {
        if (!audioSource) return;
        if (entityTypeToSound.ContainsKey(type))
        {
            audioSource.PlayOneShot(entityTypeToSound[type]);
        }
    }
}