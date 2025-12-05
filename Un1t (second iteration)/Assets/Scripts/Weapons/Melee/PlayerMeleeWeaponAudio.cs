using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeWeaponAudio : MonoBehaviour
{
    [SerializeField] private AudioClip StoneHitSound;
    private AudioSource audioSource;
    private MeleeWeaponController controller;
    private Dictionary<HitableEntityType, AudioClip> entityTypeToSound;
    private void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
        controller = GetComponent<MeleeWeaponController>();
        controller.EntityHit.AddListener(PlayHitSound);
        // TODO: import serializable dictionary asset already
        entityTypeToSound = new()
        {
            {HitableEntityType.Rock, StoneHitSound}
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