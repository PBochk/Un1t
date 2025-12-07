public class XPGainAbility : PlayerAbility
{
    public XPGainAbility(PlayerUpgradeController man) : base(man)
    {
    }

    public override void Apply()
    {
        UnlockUpgrade(PlayerUpgradeTypes.XPGain);
        UpgradeFactory.GetUpgrade(PlayerUpgradeTypes.XPGain, UpgradeTiers.x3).Apply();
        base.Apply();
    }
}