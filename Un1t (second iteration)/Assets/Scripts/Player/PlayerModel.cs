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
    private float movingSpeed;
    private float dashSpeed;
    private float dashDuration;
    private float dashCooldown;
    private int level = 1;
    private int currentXP = 0;
    private float currentHealth;
    
    //not sufficient for saving and loading
    [XmlIgnore] private readonly List<int> XPToNextLevel;

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
            currentHealth = value > 0 ? value : 0;
            HealthChanged?.Invoke();
        }
    }
    
    public float MovingSpeed => movingSpeed;
    public float DashSpeed => dashSpeed;
    public float DashDuration => dashDuration;
    public float DashCooldown => dashCooldown;   
    public int Level => level;
    public int CurrentXP
    {
        get => currentXP;
        private set
        {
            currentXP = value > 0 ? value : 0;
            ExperienceChanged?.Invoke();
        }
    }
    public int NextLevelXP => XPToNextLevel[level];
    public bool IsLevelUpAvailable => CurrentXP >= NextLevelXP && level <= XPToNextLevel.Count;

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
                       float movingSpeed, 
                       float dashSpeed, 
                       float dashDuration,
                       float dashCooldown, 
                       int level, 
                       List<int> XPToNextLevel)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        this.movingSpeed = movingSpeed;
        this.dashSpeed = dashSpeed;
        this.dashDuration = dashDuration;
        this.dashCooldown = dashCooldown;
        this.level = level;
        this.XPToNextLevel = XPToNextLevel;
    }
    
    public IActor CreateInstance()
    {
        var player = Object.Instantiate(playerPrefab);
        player.Initialize(this);
        return player;
    }

    public void TakeHeal(float heal)
    {
        if (CurrentHealth <= 0) return;
        CurrentHealth += heal;
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
        CurrentXP += increment;
    }

    public void DecreaseXP(int decrement)
    {
        CurrentXP -= decrement;
    }

    public void LevelUp()
    {
        var diff = CurrentXP - NextLevelXP; // is needed to not trigger OnExperienceChanged too early
        level++;
        CurrentXP = diff;
        LevelChanged?.Invoke();
    }

    public void UpgradeHealth(float increment)
    {
        MaxHealth += increment;
    }

    public void UpgradeMovingSpeed(float increment)
    {
        movingSpeed += increment;
    }
}