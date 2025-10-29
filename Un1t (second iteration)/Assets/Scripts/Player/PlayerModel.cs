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
            HealthChanged.Invoke();
        }
    }
    public float CurrentHealth
    {
        get => currentHealth;
        private set
        {
            currentHealth = value;
            HealthChanged.Invoke();
        }
    }
    public float MovingSpeed => movingSpeed;

    private float healthUpgrade;

    private int level;
    private int xpCoefficient;
    private int currentXP = 0;
    private int nextLevelXP;
    public int Level => level;
    public int CurrentXP
    {
        get => currentXP;
        private set
        {
            currentXP = value;
            ExperienceChanged.Invoke();
        }
    }

    public int NextLevelXP
    {
        get => nextLevelXP;
        private set
        {
            nextLevelXP = value;
            ExperienceChanged.Invoke();
        }
    }

    private PlayerTools previousTool = PlayerTools.None;
    private PlayerTools equippedTool = PlayerTools.None;
    public PlayerTools PreviousTool => previousTool;
    public PlayerTools EquippedTool => equippedTool;

    private List<PlayerTools> availableTools = new() { PlayerTools.None, PlayerTools.Melee, PlayerTools.Range, PlayerTools.Pickaxe };
    private List<PlayerTools> unlockedTools = new() { PlayerTools.None, PlayerTools.Melee, PlayerTools.Range, PlayerTools.Pickaxe };
    public List<PlayerTools> AvailableTools => availableTools;
    public List<PlayerTools> UnlockedTools => unlockedTools;

    public event Action HealthChanged;
    public event Action ExperienceChanged;
    public event Action NextLevel;
    public event Action<PlayerTools> ToolChanged;
    public PlayerModel(float maxHealth, float healthUpgrade, float movingSpeed, int level, int xpCoefficient)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        this.healthUpgrade = healthUpgrade;
        this.movingSpeed = movingSpeed;
        this.level = level;
        this.xpCoefficient = xpCoefficient;
        nextLevelXP = GetNextLevelXP();
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

    public void UpgradeHealth()
    {
        MaxHealth += healthUpgrade;
        CurrentHealth += healthUpgrade;
    }
    
    // TODO: remove
    private int GetFibonachi(int n) => n > 1 ? GetFibonachi(n - 1) + GetFibonachi(n - 2) : n;

    public void SetEquippedTool(PlayerTools tool)
    {
        (previousTool, equippedTool) = (equippedTool, tool);
        ToolChanged.Invoke(equippedTool);
    }

    public void SetPreviousEquippedTool() => SetEquippedTool(previousTool);
}