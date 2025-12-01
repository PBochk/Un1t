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
        UpgradeFactory.Manager = this;
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
            var upgradeType = availableUpgrades[Random.Range(0, availableUpgrades.Count)];
            var upgrade = UpgradeFactory.GetUpgrade(upgradeType);
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


}