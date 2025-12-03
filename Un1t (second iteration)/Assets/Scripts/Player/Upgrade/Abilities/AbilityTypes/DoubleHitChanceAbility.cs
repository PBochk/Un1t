public class DoubleHitChanceAbility : PlayerAbility
{
    public DoubleHitChanceAbility(PlayerUpgradeController man) : base(man)
    {
    }

    public override void Apply()
    {
        UnlockUpgrade(PlayerUpgradeTypes.DoubleHitChance);
        var a = UpgradeFactory.GetUpgrade(PlayerUpgradeTypes.DoubleHitChance, UpgradeTiers.x3);
        a.Apply();
        a.Apply(); // TODO: make the PlayerUpgradeTiers.Unlock tier instead of this
        base.Apply();
    }
}