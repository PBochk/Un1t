using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerModelMB))]
public class PlayerUpgradeManager : MonoBehaviour
{

    [SerializeField] private List<PlayerUpgradeTypes> availableUpgrades;
    [SerializeField] private List<PlayerAbilityTypes> availableAbilities;
    private PlayerModel playerModel;
    private PlayerMeleeWeaponModel meleeModel;
    private PlayerRangeWeaponModel rangeModel;
    private List<PlayerUpgrade> currentChoiceUpgrades = new();
    public PlayerModel PlayerModel => playerModel;
    public PlayerMeleeWeaponModel MeleeModel => meleeModel;
    public PlayerRangeWeaponModel RangeModel => rangeModel;
    public Stack<PlayerUpgrade> ReceivedUpgrades { get; private set; }

    public UnityEvent<List<PlayerUpgrade>> UpgradesChoiceSet;

    private void Start()
    {
        playerModel = GetComponent<PlayerModelMB>().PlayerModel;
        meleeModel = (PlayerMeleeWeaponModel) GetComponentInChildren<PlayerMeleeWeaponModelMB>().MeleeWeaponModel;
        rangeModel = GetComponentInChildren<PlayerRangeWeaponModelMB>().PlayerRangeWeaponModel;
        playerModel.LevelChanged += SetUpgradeChoice;
    }

    public void SetLevelUpReward()
    {
        if (playerModel.Level % 5 != 0)
        {
            SetUpgradeChoice();
        }
        else
        {
            SetAbilityChoice();
        }
    }

    public void SetUpgradeChoice()
    {
        currentChoiceUpgrades.Clear();
        for (var i = 0; i < 3; i++)
        {
            var upgradeName = availableUpgrades[Random.Range(0, availableUpgrades.Count)];
            PlayerUpgrade upgrade = null;
            // TODO: rework with dictionary
            switch (upgradeName)
            {
                case PlayerUpgradeTypes.MaxHealth:
                {
                    upgrade = new MaxHealthUpgrade(this, GetRandomTier());
                    break;
                };
                case PlayerUpgradeTypes.HealCost:
                {
                    upgrade = new HealCostUpgrade(this, GetRandomTier());
                    break;
                }
                case PlayerUpgradeTypes.XPGain:
                {
                    upgrade = new XPGainUpgrade(this, GetRandomTier());
                    break;
                }
                case PlayerUpgradeTypes.MovingSpeed:
                {
                    upgrade = new MovingSpeedUpgrade(this, GetRandomTier());
                    break;
                }
                case PlayerUpgradeTypes.MeleeSpeed:
                {
                    upgrade = new MeleeAttackSpeedUpgrade(this, GetRandomTier());
                    break;
                }
                case PlayerUpgradeTypes.MeleeDamage:
                {
                    upgrade = new MeleeDamageUpgrade(this, GetRandomTier());
                    break;
                }
                case PlayerUpgradeTypes.RangeDamage:
                {
                    upgrade = new RangeDamageUpgrade(this, GetRandomTier());
                    break;
                }
            }
            currentChoiceUpgrades.Add(upgrade);
        }
        UpgradesChoiceSet.Invoke(currentChoiceUpgrades);
        
    }

    public void SetAbilityChoice()
    {
        currentChoiceUpgrades.Clear();
        for (var i = 0; i < 3; i++)
        {
            var abilityName = availableAbilities[Random.Range(0, availableAbilities.Count)];
            PlayerAbility ability = null;
            // TODO: rework with dictionary
            switch (abilityName)
            {
                case PlayerAbilityTypes.Regeneration:
                {
                    ability = new RegenerationAbility(this);
                    break;
                }

            }
            currentChoiceUpgrades.Add(ability);
        }
        UpgradesChoiceSet.Invoke(currentChoiceUpgrades);
    }

    private UpgradeTiers GetRandomTier()
    {
        // TODO: replace magic numbers with serialized variables
        var rand = Random.Range(1, 11);
        if (rand <= 6) return UpgradeTiers.x1;
        else if (rand <= 9) return UpgradeTiers.x2;
        else return UpgradeTiers.x3;
    }
}