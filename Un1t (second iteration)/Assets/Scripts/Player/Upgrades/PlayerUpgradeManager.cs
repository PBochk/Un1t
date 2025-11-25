using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerModelMB))]
public class PlayerUpgradeManager : MonoBehaviour
{
    //[SerializeField] private PlayerModelMB playerModelMB;
    //[SerializeField] private PlayerMeleeWeaponModelMB meleeModelMB;
    //[SerializeField] private PlayerRangeWeaponModelMB rangeModelMB;
    [SerializeField] private List<PlayerUpgradeTypes> unlockedUpgrades;
    private PlayerModel playerModel;
    private PlayerMeleeWeaponModel meleeModel;
    private PlayerRangeWeaponModel rangeModel;
    private List<PlayerUpgrade> currentChoiceUps = new();
    public PlayerModel PlayerModel => playerModel;
    public PlayerMeleeWeaponModel MeleeModel => meleeModel;
    public PlayerRangeWeaponModel RangeModel => rangeModel;
    public Stack<PlayerUpgrade> ReceivedUpgrades { get; private set; }

    public UnityEvent<List<PlayerUpgrade>> UpgradesChoiceSet;

    private void Start()
    {
        playerModel = GetComponent<PlayerModelMB>().PlayerModel;
        meleeModel = (PlayerMeleeWeaponModel) GetComponentInChildren<PlayerMeleeWeaponModelMB>().MeleeWeaponModel;
        //rangeModel = rangeModelMB.PlayerRangeWeaponModel;
        playerModel.LevelChanged += SetUpgradeChoice;
    }

    public void SetUpgradeChoice()
    {
        currentChoiceUps.Clear();
        for (var i = 0; i < 3; i++)
        {
            var upgradeName = unlockedUpgrades[Random.Range(0, unlockedUpgrades.Count)];
            PlayerUpgrade upgrade = null;
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
                case PlayerUpgradeTypes.MeleeSpeed:
                {
                    upgrade = new MeleeAttackSpeedUpgrade(this, GetRandomTier());
                    break;
                }
                case PlayerUpgradeTypes.MovingSpeed:
                {
                    upgrade = new MovingSpeedUpgrade(this, GetRandomTier());
                    break;
                }
            }
            currentChoiceUps.Add(upgrade);
        }
        UpgradesChoiceSet.Invoke(currentChoiceUps);
        
    }

    private UpgradeTiers GetRandomTier()
    {
        var rand = Random.Range(1, 11);
        if (rand <= 6) return UpgradeTiers.x1;
        else if (rand <= 9) return UpgradeTiers.x2;
        else return UpgradeTiers.x3;
    }
}