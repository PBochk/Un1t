using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerModelMB))]
[RequireComponent(typeof(PlayerUpgradeModelMB))]
public class PlayerUpgradeController : MonoBehaviour
{
    public static PlayerUpgradeController Instance;
    private PlayerModel playerModel;
    private PlayerMeleeWeaponModel meleeModel;
    private PlayerRangeWeaponModel rangeModel;
    private PlayerUpgradeModel upgradeModel;
    private List<PlayerUpgrade> rewardChoice = new();
    private List<PlayerUpgradeTypes> chosenUpgradeTypes = new();
    private List<PlayerAbilityTypes> chosenAbilityTypes = new();
    public PlayerModel PlayerModel => playerModel;
    public PlayerMeleeWeaponModel MeleeModel => meleeModel;
    public PlayerRangeWeaponModel RangeModel => rangeModel;
    public PlayerUpgradeModel UpgradeModel => upgradeModel;
    public UnityEvent<List<PlayerUpgrade>> UpgradesChoiceSet;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerModel = GetComponent<PlayerModelMB>().PlayerModel;
        meleeModel = (PlayerMeleeWeaponModel) GetComponentInChildren<PlayerMeleeWeaponModelMB>().MeleeWeaponModel;
        rangeModel = GetComponentInChildren<PlayerRangeWeaponModelMB>().RangeWeaponModel;
        upgradeModel = GetComponent<PlayerUpgradeModelMB>().PlayerUpgradeModel;
        playerModel.LevelChanged += SetRewardChoice;
        UpgradeFactory.Manager = this;
        AbilityFactory.Manager = this;
    }

    private void SetRewardChoice()
    {
        ClearPreviousChoice();
        if (playerModel.Level % 5 == 0 && upgradeModel.AvailableAbilities.Count > 0)
        {
            SetAbilityChoice();
        }
        else
        {
            SetUpgradeChoice();
        }
        UpgradesChoiceSet.Invoke(rewardChoice);
    }
    private void ClearPreviousChoice()
    {
        if((playerModel.Level - 1) % 5 == 0)
        {
            foreach (PlayerAbility ability in rewardChoice.Cast<PlayerAbility>())
            {
                ability.RemoveListeners();
            }
            chosenAbilityTypes.Clear();
        }
        chosenUpgradeTypes.Clear();
        rewardChoice.Clear();
    }

    private void SetUpgradeChoice()
    {
        while(rewardChoice.Count < 3)
        {
            var upgradeType = upgradeModel.UnlockedUpgrades[Random.Range(0, upgradeModel.UnlockedUpgrades.Count)];
            if (chosenUpgradeTypes.Contains(upgradeType)) continue;
            var upgrade = UpgradeFactory.GetUpgrade(upgradeType);
            chosenUpgradeTypes.Add(upgradeType);
            rewardChoice.Add(upgrade);
        }
    }

    private void SetAbilityChoice()
    {
        while (chosenAbilityTypes.Count < 3)
        {
            var abilityType = upgradeModel.AvailableAbilities[Random.Range(0, upgradeModel.AvailableAbilities.Count)];
            if (chosenAbilityTypes.Contains(abilityType)) continue;
            var ability = AbilityFactory.GetAbility(abilityType);
            chosenAbilityTypes.Add(abilityType);
            rewardChoice.Add(ability);
            ability.AbilityApplied += () => upgradeModel.RemoveAbility(abilityType);
        }
    }
}