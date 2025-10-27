using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerModel
{
    private float maxHealth;
    private float currentHealth;
    private float movingSpeed;
    public float MaxHealth
    { 
        get => maxHealth;
        private set
        {
            maxHealth = value;
            HealthChange.Invoke();
        }
    }
    public float CurrentHealth
    {
        get => currentHealth;
        private set
        {
            currentHealth = value;
            HealthChange.Invoke();
        }
    }
    public float MovingSpeed => movingSpeed;

    private float healthUpgrade;


    private int level;
    private int xpCoefficient;
    private int currentXP;
    private int nextLevelXP;
    public int Level => level;
    public float CurrentXP => currentXP;
    public float NextLevelXP => nextLevelXP;

    private List<PlayerTools> availableTools = new() { PlayerTools.None, PlayerTools.Melee, PlayerTools.Range, PlayerTools.Pickaxe };
    private List<PlayerTools> unlockedTools = new() { PlayerTools.None, PlayerTools.Melee, PlayerTools.Range, PlayerTools.Pickaxe };
    public List<PlayerTools> AvailableTools => availableTools;
    public List<PlayerTools> UnlockedTools => unlockedTools;

    public event Action NextLevel;
    public event Action HealthChange;

    public PlayerModel(float maxHealth, float healthUpgrade, float movingSpeed, int level, int xpCoefficient)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        this.healthUpgrade = healthUpgrade;
        this.movingSpeed = movingSpeed;
        this.level = level;
        this.xpCoefficient = xpCoefficient;
        SetNextLevelXP();
    }

    public void TakeHeal(float heal)
    {
        CurrentHealth += heal;
    }

    public void TakeDamage(float decrement)
    {
        CurrentHealth -= decrement;

    }

    public void AddXP(int increment)
    {
        currentXP += increment;
        CheckXP();
    }

    private void CheckXP()
    {
        if (currentXP >= nextLevelXP)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        SetNextLevelXP();
        NextLevel?.Invoke();
    }

    private void SetNextLevelXP()
    {
        nextLevelXP = GetFibonachi(level) * xpCoefficient;
    }

    public void UpgradeHealth()
    {
        MaxHealth += healthUpgrade;
        CurrentHealth += healthUpgrade;
    }
    
    private int GetFibonachi(int n) => n > 1 ? GetFibonachi(n - 1) + GetFibonachi(n - 2) : n;
}