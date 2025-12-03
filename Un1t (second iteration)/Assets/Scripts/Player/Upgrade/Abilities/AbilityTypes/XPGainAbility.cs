public class XPGainAbility : PlayerAbility
{
    public XPGainAbility(PlayerUpgradeController man) : base(man)
    {
    }

    public override void Apply()
    {
        UnlockUpgrade(PlayerUpgradeTypes.XPGain);
        base.Apply();
    }
}