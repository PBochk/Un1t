using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private PlayerModelMB playerModelMB;
    //[SerializeField] private PlayerMeleeWeaponModelMB meleeModelMB;
    //[SerializeField] private PlayerRangeWeaponModelMB rangeModelMB;
    [SerializeField] private List<PlayerUpgrades> unlockedUpgrades;
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
        playerModel = playerModelMB.PlayerModel;
        //meleeModel = (PlayerMeleeWeaponModel) meleeModelMB.MeleeWeaponModel;
        //rangeModel = rangeModelMB.PlayerRangeWeaponModel;
        unlockedUpgrades = new()
        {
            PlayerUpgrades.MaxHealth,
        };
        playerModel.NextLevel += SetUpgradeChoice;
    }

    public void SetUpgradeChoice()
    {
        currentChoiceUps.Clear();
        for (var i = 0; i < 3; i++)
        {
            var upgradeName = unlockedUpgrades[Random.Range(0, unlockedUpgrades.Count - 1)];
            PlayerUpgrade upgrade = null;
            switch (upgradeName)
            {
                case PlayerUpgrades.MaxHealth:
                {
                    upgrade = new MaxHealthUpgrade(this, GetRandomTier());
                    break;
                };
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