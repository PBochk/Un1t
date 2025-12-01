using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerModel : IInstanceModel
{
    [XmlIgnore] private const string PREFAB_NAME = "PlayerWithGun";
    [XmlIgnore] private static readonly PlayerModelMB playerPrefab;
        
    //sufficient for saving and loading
    private float maxHealth;
    private float currentHealth;
    private float regenPerSecond = 0;
    private int level = 1;
    private float currentXP = 0;
    private float healCostCoefficient = 0.5f;
    private float xpGainCoefficient = 1f;
    private float movingSpeed;
    private float dashSpeed;
    private float dashDuration;
    private float dashCooldown;
    
    //not sufficient for saving and loading
    [XmlIgnore] private readonly List<float> XPToNextLevel;

    public float MaxHealth
    { 
        get => maxHealth;
        private set
        {
            maxHealth = value;
            HealthChanged?.Invoke();
        }
    }
    
    public float CurrentHealth
    {
        get => currentHealth;
        private set
        {
            if(value < 0) currentHealth = 0;
            else if (value > maxHealth) currentHealth = maxHealth;
            else currentHealth = value;
            HealthChanged?.Invoke();
        }
    }
    public float RegenPerSecond => regenPerSecond;
    public int Level => level;
    public float CurrentXP
    {
        get => currentXP;
        private set
        {
            currentXP = value > 0 ? value : 0;
            ExperienceChanged?.Invoke();
        }
    }
    public float NextLevelXP => XPToNextLevel[level];
    public bool IsLevelUpAvailable => CurrentXP >= NextLevelXP && level <= XPToNextLevel.Count;
    public float HealCostInXP => NextLevelXP * healCostCoefficient;
    public float MovingSpeed => movingSpeed;
    public float DashSpeed => dashSpeed;
    public float DashDuration => dashDuration;
    public float DashCooldown => dashCooldown;   

    public event Action HealthChanged;
    public event Action DamageTaken;
    public event Action PlayerDeath;
    public event Action<bool> PlayerRestrained;
    public event Action ExperienceChanged;
    public event Action LevelChanged;

    static PlayerModel()
    {
        playerPrefab = Resources.Load<PlayerModelMB>(PREFAB_NAME);
    }
    
    public PlayerModel(float maxHealth, 
                       int level, 
                       List<float> XPToNextLevel,
                       float healCostCoefficient,
                       float xpGainCoefficient,
                       float movingSpeed, 
                       float dashSpeed, 
                       float dashDuration,
                       float dashCooldown)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        this.level = level;
        this.XPToNextLevel = XPToNextLevel;
        this.healCostCoefficient = healCostCoefficient;
        this.xpGainCoefficient = xpGainCoefficient;
        this.movingSpeed = movingSpeed;
        this.dashSpeed = dashSpeed;
        this.dashDuration = dashDuration;
        this.dashCooldown = dashCooldown;
    }
    
    public IActor CreateInstance()
    {
        var player = Object.Instantiate(playerPrefab);
        player.Initialize(this);
        return player;
    }

    public void TakeHeal(float heal, float xpCost = 0)
    {
        if (CurrentHealth <= 0 || CurrentHealth == MaxHealth || CurrentXP < xpCost) return;
        CurrentHealth += heal;
        CurrentXP -= xpCost;
    }

    public void TakeDamage(float decrement)
    {
        if (CurrentHealth <= 0) return;
        CurrentHealth -= decrement;
        CheckHealth();
        DamageTaken?.Invoke();
    }

    private void CheckHealth()
    {
        if (CurrentHealth <= 0)
        {
            SetPlayerRestrained(true);
            PlayerDeath?.Invoke();
        }
    }

    public void SetPlayerRestrained(bool isRestrained)
    {
        PlayerRestrained?.Invoke(isRestrained);
    }

    public void IncreaseXP(int increment)
    {
        CurrentXP += increment * xpGainCoefficient;
    }

    public void DecreaseXP(int decrement)
    {
        CurrentXP -= decrement;
    }

    public void LevelUp()
    {
        if (level >= XPToNextLevel.Count - 1) return;
        var diff = CurrentXP - NextLevelXP; // is needed to not trigger OnExperienceChanged too early
        level++;
        CurrentXP = diff;
        LevelChanged?.Invoke();
    }

    public void UpgradeHealth(float increment)
    {
        MaxHealth += increment;
    }

    public void UpgradeRegeneration(float increment)
    {
        regenPerSecond += increment;
    }

    public void UpgradeHealCost(float decrement)
    {
        healCostCoefficient = healCostCoefficient - decrement < 0 ? 0 : healCostCoefficient - decrement;
    }
    public void UpgradeXPGain(float increment)
    {
        xpGainCoefficient += increment;
    }
    public void UpgradeMovingSpeed(float increment)
    {
        movingSpeed += increment;
    }
}