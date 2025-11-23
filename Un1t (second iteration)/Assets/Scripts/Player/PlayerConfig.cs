using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/Player")]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] private float baseMaxHealth;
    [SerializeField] private float baseMovingSpeed;
    [SerializeField] private float baseDashSpeed;
    [SerializeField] private float baseDashDuration;
    [SerializeField] private float baseDashCooldown;
    [SerializeField] private int level;
    [Tooltip("How much XP player needs to get level")]
    [SerializeField] private List<int> xpToNextLevel; // can't use IReadOnlyList because it doesn't support serialization
    public float BaseMaxHealth => baseMaxHealth;
    public float BaseMovingSpeed => baseMovingSpeed;
    public float BaseDashSpeed => baseDashSpeed;
    public float BaseDashDuration => baseDashDuration;
    public float BaseDashCooldown => baseDashCooldown;
    public int Level => level;
    public List<int> XPToNextLevel => xpToNextLevel;
}
