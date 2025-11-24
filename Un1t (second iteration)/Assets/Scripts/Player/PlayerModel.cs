using System;
using System.Collections.Generic;
using System.Xml.Serialization;
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
    private float healthUpgrade; //TODO: rework upgrade system
    private int level = 1;
    private int currentXP = 0;
    private float currentHealth;
    
    //not sufficient for saving and loading
    [XmlIgnore] private bool isRestrained; //TODO: move to controller 

    [XmlIgnore] private readonly List<int> XPToNextLevel; // can't use IReadOnlyList because it doesn't support serialization (in config)

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

    public bool IsRestrained
    {
        get => isRestrained;
        private set
        {
            isRestrained = value;
            PlayerRestrained?.Invoke();
        }
    }
    
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
    
    public event Action HealthChanged;
    public event Action DamageTaken;
    public event Action PlayerDeath;
    public event Action PlayerRestrained;
    public event Action ExperienceChanged;
    public event Action NextLevel;

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

    // TODO: move to controller
    public void SetPlayerRestrained(bool isRestrained)
    {
        IsRestrained = isRestrained;
    }

    public void IncreaseXP(int increment)
    {
        CurrentXP += increment;
        CheckXP();
    }

    public void DecreaseXP(int decrement)
    {
        CurrentXP -= decrement;
    }

    private void CheckXP()
    {
        if (CurrentXP >= NextLevelXP && level <= XPToNextLevel.Count)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        CurrentXP = 0;
        NextLevel?.Invoke();
    }

    // TODO: rework upgrades
    public void UpgradeHealth()
    {
        MaxHealth += healthUpgrade;
        CurrentHealth += healthUpgrade;
    }

    public void UpgradeHealth(float increment)
    {
        MaxHealth += increment;
    }
}