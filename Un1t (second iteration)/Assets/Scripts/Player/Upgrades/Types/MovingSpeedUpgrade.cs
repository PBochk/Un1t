using UnityEngine;

public class MovingSpeedUpgrade : PlayerUpgrade
{
    public MovingSpeedUpgrade(PlayerUpgradeManager man, UpgradeTiers tier) : base(man, tier)
    {
    }

    public override void Apply()
    {
        UpgradeManager.PlayerModel.UpgradeMovingSpeed(UpgradeValue);
    }
}
