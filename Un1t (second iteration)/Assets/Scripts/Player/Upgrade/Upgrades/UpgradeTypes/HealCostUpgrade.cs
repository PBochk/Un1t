using System.Collections.Generic;
using UnityEngine;

public class HealCostUpgrade : PlayerUpgrade
{
    public HealCostUpgrade(PlayerUpgradeManager man, UpgradeTiers tier) : base(man, tier)
    {
    }

    public override void Apply()
    {
        UpgradeManager.PlayerModel.UpgradeHealCost(UpgradeValue);
    }
}