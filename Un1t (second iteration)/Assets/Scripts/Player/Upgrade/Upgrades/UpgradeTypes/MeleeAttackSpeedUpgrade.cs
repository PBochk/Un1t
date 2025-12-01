public class MeleeAttackSpeedUpgrade : PlayerUpgrade
{
    public MeleeAttackSpeedUpgrade(PlayerUpgradeManager man, UpgradeTiers tier) : base(man, tier)
    {
    }

    public override void Apply()
    {
        UpgradeManager.MeleeModel.UpgradeAttackSpeed(UpgradeValue);
    }
}