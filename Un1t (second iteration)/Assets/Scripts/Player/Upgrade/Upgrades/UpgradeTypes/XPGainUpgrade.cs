public class XPGainUpgrade : PlayerUpgrade
{
    public XPGainUpgrade(PlayerUpgradeController man, UpgradeTiers tier) : base(man, tier)
    {
    }

    public override void Apply()
    {
        UpgradeManager.PlayerModel.UpgradeXPGain(UpgradeValue);
    }
}