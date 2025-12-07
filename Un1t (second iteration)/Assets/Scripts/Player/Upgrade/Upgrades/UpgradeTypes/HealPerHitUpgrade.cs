public class HealPerHitUpgrade : PlayerUpgrade
{
    public HealPerHitUpgrade(PlayerUpgradeController man, UpgradeTiers tier) : base(man, tier)
    {
    }

    public override void Apply()
    {
        UpgradeManager.PlayerModel.UpgradeHealPerHit(UpgradeValue);
    }
}