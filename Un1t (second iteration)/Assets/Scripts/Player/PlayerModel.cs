using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using Object = UnityEngine.Object;

public class PlayerModel : IInstanceModel
{
    [XmlIgnore] private const string PREFAB_NAME = "PlayerWithGun";
    [XmlIgnore] private static readonly PlayerModelMB playerPrefab;
        
    private float maxHealth;
    private float currentHealth;
    private float movingSpeed;
    private bool isRestrained;
    private bool isDead = false;
    private bool isInvulnerable = false;
    private float healthUpgrade;
    private int level;
    private int xpCoefficient;
    private int currentXP = 0;
    private int nextLevelXP;
    private PlayerTools previousTool = PlayerTools.None;
    private PlayerTools equippedTool = PlayerTools.None;
    private List<PlayerTools> availableTools = new() { PlayerTools.None, PlayerTools.Melee, PlayerTools.Range, PlayerTools.Pickaxe };
    
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
    public bool IsInvulnerable => isInvulnerable;
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
    public PlayerTools PreviousTool => previousTool;
    public PlayerTools EquippedTool => equippedTool;
    public List<PlayerTools> AvailableTools => availableTools;

    public event Action HealthChanged;
    public event Action PlayerDeath;
    public event Action PlayerRestrained;
    public event Action ExperienceChanged;
    public event Action NextLevel;
    public event Action<PlayerTools> ToolChanged;

    static PlayerModel()
    {
        playerPrefab = Resources.Load<PlayerModelMB>(PREFAB_NAME);
    }
        
    
    //TODO: PlayerModel initialization with base values (config is not done yet)
    public PlayerModel()
    {
    }

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
        if (isDead) return;
        CurrentHealth += heal;
    }

    public void TakeDamage(float decrement)
    {
        if(isDead) return;
        CurrentHealth -= decrement;
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (CurrentHealth <= 0)
        {
            isDead = true;
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
        ToolChanged?.Invoke(equippedTool);
    }

    public void SetPreviousEquippedTool() => SetEquippedTool(previousTool);
    
    public IActor CreateInstance()
    {
        var player = Object.Instantiate(playerPrefab);
        player.Initialize(this);
        return player;
    }
}