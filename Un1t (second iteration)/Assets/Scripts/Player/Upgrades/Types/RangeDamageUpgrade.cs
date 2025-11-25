public class RangeDamageUpgrade : PlayerUpgrade
{
    public RangeDamageUpgrade(PlayerUpgradeManager man, UpgradeTiers tier) : base(man, tier)
    {
    }

    public override void Apply()
    {
        UpgradeManager.RangeModel.UpgradeDamage(UpgradeValue);
    }
}