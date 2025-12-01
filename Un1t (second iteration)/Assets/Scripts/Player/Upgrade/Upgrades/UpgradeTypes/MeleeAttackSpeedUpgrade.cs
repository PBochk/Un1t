public class MeleeAttackSpeedUpgrade : PlayerUpgrade
{
    public MeleeAttackSpeedUpgrade(PlayerUpgradeController man, UpgradeTiers tier) : base(man, tier)
    {
    }

    public override void Apply()
    {
        UpgradeManager.MeleeModel.UpgradeAttackSpeed(UpgradeValue);
    }
}