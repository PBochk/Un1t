public class DoubleHitChanceUpgrade : PlayerUpgrade
{
    public DoubleHitChanceUpgrade(PlayerUpgradeController man, UpgradeTiers tier) : base(man, tier)
    {
    }

    public override void Apply()
    {
        UpgradeManager.MeleeModel.UpgradeDoubleHitChance(UpgradeValue);
    }
}