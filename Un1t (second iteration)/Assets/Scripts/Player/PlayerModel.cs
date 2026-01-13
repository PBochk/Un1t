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
    [XmlIgnore] private PlayerConfig config;
    private PlayerMeleeWeaponModel meleeModel;
    private PlayerRangeWeaponModel rangeModel;
    private PlayerUpgradeModel upgradeModel;
    //sufficient for saving and loading
    private float maxHealth;
    private float currentHealth;
    private float regenPerSecond = 0;
    private int level = 1;
    private float currentXP = 0;
    private float healPerHit = 0;
    private float healCostCoefficient = 0.5f;
    private float xpGainCoefficient = 1f;
    private float resistCoefficient = 1f;
    private float dodgeChance = 0;
    private float shieldCooldown = 0;
    private float movingSpeed;
    private float dashSpeed;
    private float dashDuration;
    private float dashCooldown;
    
    //not sufficient for saving and loading
    [XmlIgnore] private readonly List<float> XPToNextLevel;

    public PlayerMeleeWeaponModel MeleeModel => meleeModel;
    public PlayerRangeWeaponModel RangeModel => rangeModel;
    public PlayerUpgradeModel UpgradeModel => upgradeModel;

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
    public float DodgeChance => dodgeChance;
    public float ShieldCooldown => shieldCooldown;
    public float MovingSpeed => movingSpeed;
    public float DashSpeed => dashSpeed;
    public float DashDuration => dashDuration;
    public float DashCooldown => dashCooldown;
    //public float DodgeCooldown => dodgeCooldown;

    public event Action HealthChanged;
    public event Action DamageTaken;
    public event Action PlayerDeath;
    public event Action ExperienceChanged;
    public event Action LevelChanged;
    public event Action ShieldUnlocked;

    static PlayerModel()
    {
        playerPrefab = Resources.Load<PlayerModelMB>(PREFAB_NAME);
    }

    public PlayerModel()
    {
    }
    
    public PlayerModel(PlayerConfig config)
    {
        this.config = config;
        maxHealth = config.BaseMaxHealth;
        currentHealth = maxHealth;
        level = config.Level;
        XPToNextLevel = config.XPToNextLevel;
        healPerHit = config.BaseHealPerHit;
        healCostCoefficient = config.BaseHealCostCoefficient;
        xpGainCoefficient = config.BaseXPGainCoefficient;
        resistCoefficient = config.BaseResistCoefficient;
        dodgeChance = config.BaseDodgeChance;
        shieldCooldown = config.BaseShieldCooldown;
        movingSpeed = config.BaseMovingSpeed;
        dashSpeed = config.BaseDashSpeed;
        dashDuration = config.BaseDashDuration;
        dashCooldown = config.BaseDashCooldown;
    }
    
    public void BindModels(PlayerMeleeWeaponModel meleeModel, PlayerRangeWeaponModel rangeModel, PlayerUpgradeModel upgradeModel)
    {
        this.meleeModel = meleeModel;
        this.rangeModel = rangeModel;
        this.upgradeModel = upgradeModel;
        Debug.Log($"Models are null: {meleeModel == null}");
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

    public void Regenerate() => TakeHeal(regenPerSecond);
    public void HealByHit() => TakeHeal(healPerHit);

    public void TakeDamage(float decrement)
    {
        if (CurrentHealth <= 0) return;
        CurrentHealth -= decrement * resistCoefficient;
        CheckHealth();
        DamageTaken?.Invoke();
    }

    private void CheckHealth()
    {
        if (CurrentHealth <= 0)
        {
            PlayerDeath?.Invoke();
        }
    }

    public void IncreaseXP(float increment)
    {
        CurrentXP += increment * xpGainCoefficient;
    }

    public void DecreaseXP(float decrement)
    {
        if (CurrentXP <= 0) return;
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
    public void UpgradeHealPerHit(float increment)
    {
        healPerHit += increment;
    }
    public void UpgradeHealCost(float decrement)
    {
        healCostCoefficient = healCostCoefficient - decrement < 0 ? 0 : healCostCoefficient - decrement;
    }
    public void UpgradeXPGain(float increment)
    {
        xpGainCoefficient += increment;
    }
    public void UpgradeResist(float decrement)
    {
        resistCoefficient -= decrement;
    }
    public void UpgradeDodgeChance(float increment)
    {
        dodgeChance += increment;
    }
    public void UpgradeMovingSpeed(float multiplier)
    {
        movingSpeed += config.BaseMovingSpeed * multiplier;
    }
    public void UnlockShield(float shieldCooldown)
    {
        this.shieldCooldown = shieldCooldown;
        ShieldUnlocked?.Invoke();
    }

    public PlayerSaveData ToSaveData()
    {
        Debug.Log($"MeleeModel is null: {meleeModel == null}");
        var data = new PlayerSaveData();
        var rangedData = rangeModel.ToSaveData();
        var meleeData = meleeModel.ToSaveData();
        var upgradeData = upgradeModel.ToSaveData();
        Debug.Log($"MeleeData is null: {meleeData == null}");
        data.currentHealth =  currentHealth;
        data.maxHealth = maxHealth;
        data.regenPerSecond = regenPerSecond;
        data.level = level;
        data.currentXP = currentXP;
        data.healPerHit = healPerHit;
        data.healCostCoefficient = healCostCoefficient;
        data.xpGainCoefficient = xpGainCoefficient;
        data.resistCoefficient = resistCoefficient;
        data.dodgeChance = dodgeChance;
        data.shieldCooldown = shieldCooldown;
        data.movingSpeed = movingSpeed;
        data.dashSpeed = dashSpeed;
        data.dashDuration = dashDuration;
        data.dashCooldown  = dashCooldown;
        data.meleeData = meleeData;
        data.rangedData = rangedData;
        data.upgradeData = upgradeData;
        return data;
    }

    public void FromSaveData(PlayerSaveData data)
    {
        currentHealth = data.currentHealth;
        maxHealth = data.maxHealth;
        regenPerSecond = data.regenPerSecond;
        level = data.level;
        currentXP = data.currentXP;
        healPerHit = data.healPerHit;
        healCostCoefficient = data.healCostCoefficient;
        xpGainCoefficient = data.xpGainCoefficient;
        resistCoefficient = data.resistCoefficient;
        dodgeChance = data.dodgeChance;
        shieldCooldown = data.shieldCooldown;
        movingSpeed = data.movingSpeed;
        dashSpeed = data.dashSpeed;
        dashDuration = data.dashDuration;
        dashCooldown  = data.dashCooldown;
        Debug.Log($"meleeModel is null: {meleeModel == null}");
        meleeModel.FromSaveData(data.meleeData);
        rangeModel.FromSaveData(data.rangedData);
    }
}