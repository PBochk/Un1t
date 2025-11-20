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
    private float healthUpgrade; //TODO: rework upgrade system
    private int level = 1;
    private int currentXP = 0;
    private float currentHealth;
    
    //not sufficient for saving and loading
    [XmlIgnore] private bool isRestrained;
    
    //this could be a computed property
    [XmlIgnore] private int nextLevelXP;
    
    //wtf, why isn't this const
    [XmlIgnore] private int xpCoefficient;
    
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
            currentXP = value;
            ExperienceChanged?.Invoke();
        }
    }
    
    public int NextLevelXP
    {
        get => nextLevelXP;
        private set
        {
            nextLevelXP = value;
            ExperienceChanged?.Invoke();
        }
    }
    
    public event Action HealthChanged;
    public event Action PlayerDeath;
    public event Action PlayerRestrained;
    public event Action ExperienceChanged;
    public event Action NextLevel;

    static PlayerModel()
    {
        playerPrefab = Resources.Load<PlayerModelMB>(PREFAB_NAME);
    }
        
    
    public PlayerModel(float maxHealth, float movingSpeed)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        this.movingSpeed = movingSpeed;
    }

    //TODO: replace with initialization from scriptable object
    //public PlayerModel(float maxHealth, float healthUpgrade, float movingSpeed, int level, int xpCoefficient)
    //{
    //    this.maxHealth = maxHealth;
    //    currentHealth = maxHealth;
    //    this.healthUpgrade = healthUpgrade;
    //    this.movingSpeed = movingSpeed;
    //    this.level = level;
    //    this.xpCoefficient = xpCoefficient;
    //    nextLevelXP = GetNextLevelXP();
    //}

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
        IsRestrained = isRestrained;
    }

    public void AddXP(int increment)
    {
        CurrentXP += increment;
        CheckXP();
    }

    private void CheckXP()
    {
        if (CurrentXP >= nextLevelXP)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        NextLevelXP = GetNextLevelXP();
        NextLevel?.Invoke();
    }

    // TODO: replace with scriptable object
    private int GetNextLevelXP()
    {
        return GetFibonachi(level + 1) * xpCoefficient;
    }

    // TODO: rework upgrades
    public void UpgradeHealth()
    {
        MaxHealth += healthUpgrade;
        CurrentHealth += healthUpgrade;
    }
    
    // TODO: remove
    private int GetFibonachi(int n) => n > 1 ? GetFibonachi(n - 1) + GetFibonachi(n - 2) : n;
    
    public IActor CreateInstance()
    {
        var player = Object.Instantiate(playerPrefab);
        player.Initialize(this);
        return player;
    }
}