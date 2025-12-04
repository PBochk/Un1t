using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/Player")]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] private float baseMaxHealth;
    [SerializeField] private int level;
    [Tooltip("How much XP player needs to get level")]
    [SerializeField] private List<float> xpToNextLevel; // can't use IReadOnlyList because it doesn't support serialization
    [SerializeField] private float baseHealPerHit;
    [Tooltip("What part of NextLevelXP will be spent on healing")]
    [SerializeField, Range(0.1f, 1f)] private float baseHealCostCoefficient;
    [Tooltip("Multiplies all gained XP")]
    [SerializeField, Range(1f, 5f)] private float baseXPGainCoefficient;
    [SerializeField, Range(1f, 5f)] private float baseResistCoefficient;
    [SerializeField, Range(0, 1f)] private float baseDodgeChance;
    [SerializeField, Range(0, 10f)] private float baseShieldCooldown;
    [SerializeField] private float baseMovingSpeed;
    [SerializeField] private float baseDashSpeed;
    [SerializeField] private float baseDashDuration;
    [SerializeField] private float baseDashCooldown;
    public float BaseMaxHealth => baseMaxHealth;
    public int Level => level;
    public List<float> XPToNextLevel => xpToNextLevel;
    public float BaseHealPerHit => baseHealPerHit;
    public float BaseHealCostCoefficient => baseHealCostCoefficient;
    public float BaseXPGainCoefficient => baseXPGainCoefficient;
    public float BaseResistCoefficient => baseResistCoefficient;
    public float BaseDodgeChance => baseDodgeChance;
    public float BaseShieldCooldown => baseShieldCooldown;
    public float BaseMovingSpeed => baseMovingSpeed;
    public float BaseDashSpeed => baseDashSpeed;
    public float BaseDashDuration => baseDashDuration;
    public float BaseDashCooldown => baseDashCooldown;
}
