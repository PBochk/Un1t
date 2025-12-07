public class RangeDamageUpgrade : PlayerUpgrade
{
    public RangeDamageUpgrade(PlayerUpgradeController man, UpgradeTiers tier) : base(man, tier)
    {
    }

    public override void Apply()
    {
        UpgradeManager.RangeModel.UpgradeDamage(UpgradeValue);
    }
}