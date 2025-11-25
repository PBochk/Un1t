public class XPGainUpgrade : PlayerUpgrade
{
    public XPGainUpgrade(PlayerUpgradeManager man, UpgradeTiers tier) : base(man, tier)
    {
    }

    public override void Apply()
    {
        UpgradeManager.PlayerModel.UpgradeXPGain(UpgradeValue);
    }
}