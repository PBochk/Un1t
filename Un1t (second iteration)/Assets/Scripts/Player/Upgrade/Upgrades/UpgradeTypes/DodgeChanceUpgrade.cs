public class DodgeChanceUpgrade : PlayerUpgrade
{
    public DodgeChanceUpgrade(PlayerUpgradeController man, UpgradeTiers tier) : base(man, tier)
    {
    }

    public override void Apply()
    {
        UpgradeManager.PlayerModel.UpgradeDodgeChance(UpgradeValue);
    }
}