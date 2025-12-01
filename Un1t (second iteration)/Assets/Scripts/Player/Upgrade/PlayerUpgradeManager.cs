using System;
using System.Collections.Generic;
using System.Linq;
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
    private List<PlayerUpgrade> rewardChoice = new();
    public PlayerModel PlayerModel => playerModel;
    public PlayerMeleeWeaponModel MeleeModel => meleeModel;
    public PlayerRangeWeaponModel RangeModel => rangeModel;
    public Stack<PlayerUpgrade> ReceivedUpgrades { get; private set; }

    public UnityEvent<List<PlayerUpgrade>> UpgradesChoiceSet;

    private void Start()
    {
        playerModel = GetComponent<PlayerModelMB>().PlayerModel;
        meleeModel = (PlayerMeleeWeaponModel) GetComponentInChildren<PlayerMeleeWeaponModelMB>().MeleeWeaponModel;
        rangeModel = GetComponentInChildren<PlayerRangeWeaponModelMB>().RangeWeaponModel;
        playerModel.LevelChanged += SetRewardChoice;
        UpgradeFactory.Manager = this;
        AbilityFactory.Manager = this;
    }

    private void SetRewardChoice()
    {
        ClearPreviousChoice();
        if (playerModel.Level % 5 == 0 && availableAbilities.Count > 0)
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
        rewardChoice.Clear();
    }

    private void SetUpgradeChoice()
    {
        for (var i = 0; i < 3; i++)
        {
            var upgradeType = availableUpgrades[Random.Range(0, availableUpgrades.Count)];
            var upgrade = UpgradeFactory.GetUpgrade(upgradeType);
            rewardChoice.Add(upgrade);
        }
    }

    private void SetAbilityChoice()
    {
        for (var i = 0; i < 3; i++)
        {
            var abilityType = availableAbilities[Random.Range(0, availableAbilities.Count)];
            var ability = AbilityFactory.GetAbility(abilityType);
            rewardChoice.Add(ability);
            ability.AbilityApplied += () => availableAbilities.Remove(abilityType);
        }
    }

}