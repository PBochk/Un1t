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

    private PlayerModel playerModel;
    private PlayerMeleeWeaponModel meleeModel;
    private PlayerRangeWeaponModel rangeModel;
    private PlayerUpgradeModel upgradeModel;
    private List<PlayerUpgrade> rewardChoice = new();
    private List<PlayerUpgradeTypes> chosenTypes = new();
    public PlayerModel PlayerModel => playerModel;
    public PlayerMeleeWeaponModel MeleeModel => meleeModel;
    public PlayerRangeWeaponModel RangeModel => rangeModel;
    public PlayerUpgradeModel UpgradeModel => upgradeModel;
    public UnityEvent<List<PlayerUpgrade>> UpgradesChoiceSet;


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
        }
        chosenTypes.Clear();
        rewardChoice.Clear();
    }

    private void SetUpgradeChoice()
    {
        while(rewardChoice.Count < 3)
        {
            var upgradeType = upgradeModel.UnlockedUpgrades[Random.Range(0, upgradeModel.UnlockedUpgrades.Count)];
            if (chosenTypes.Contains(upgradeType)) continue;
            var upgrade = UpgradeFactory.GetUpgrade(upgradeType);
            chosenTypes.Add(upgradeType);
            rewardChoice.Add(upgrade);
        }
    }

    private void SetAbilityChoice()
    {
        for (var i = 0; i < 3; i++)
        {
            var abilityType = upgradeModel.AvailableAbilities[Random.Range(0, upgradeModel.AvailableAbilities.Count)];
            var ability = AbilityFactory.GetAbility(abilityType);
            rewardChoice.Add(ability);
            ability.AbilityApplied += () => upgradeModel.RemoveAbility(abilityType);
        }
    }
}