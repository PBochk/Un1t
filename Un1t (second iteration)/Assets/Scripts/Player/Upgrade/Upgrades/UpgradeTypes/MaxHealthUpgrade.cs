using System.Collections.Generic;
using UnityEngine;

public class MaxHealthUpgrade : PlayerUpgrade
{
    public MaxHealthUpgrade(PlayerUpgradeController man, UpgradeTiers tier) : base(man, tier)
    {
    }

    public override void Apply()
    {
        UpgradeManager.PlayerModel.UpgradeHealth(UpgradeValue);
    }
}