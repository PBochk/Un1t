using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/Player")]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] private float baseMaxHealth;
    [SerializeField] private float baseMovingSpeed;
    [Tooltip("How much XP player needs to get level")]
    [SerializeField] private int level;
    [SerializeField] private List<int> xpToNextLevel;
    public float BaseMaxHealth => baseMaxHealth;
    public float BaseMovingSpeed => baseMovingSpeed;
    public int Level => level;
    public List<int> XPToNextLevel => xpToNextLevel;
}
