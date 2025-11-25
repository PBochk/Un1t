public class MeleeDamageUpgrade : PlayerUpgrade
{
    public MeleeDamageUpgrade(PlayerUpgradeManager man, UpgradeTiers tier) : base(man, tier)
    {
    }

    public override void Apply()
    {
        UpgradeManager.MeleeModel.UpgradeDamage(UpgradeValue);
    }
}