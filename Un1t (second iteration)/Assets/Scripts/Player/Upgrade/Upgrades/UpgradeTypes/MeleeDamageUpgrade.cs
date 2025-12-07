public class MeleeDamageUpgrade : PlayerUpgrade
{
    public MeleeDamageUpgrade(PlayerUpgradeController man, UpgradeTiers tier) : base(man, tier)
    {
    }

    public override void Apply()
    {
        UpgradeManager.MeleeModel.UpgradeDamage(UpgradeValue);
    }
}