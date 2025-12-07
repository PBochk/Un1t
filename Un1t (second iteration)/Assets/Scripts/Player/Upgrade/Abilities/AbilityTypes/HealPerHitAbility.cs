public class HealPerHitAbility : PlayerAbility
{
    public HealPerHitAbility(PlayerUpgradeController man) : base(man)
    {
    }

    public override void Apply()
    {
        UnlockUpgrade(PlayerUpgradeTypes.HealPerHit);
        UpgradeFactory.GetUpgrade(PlayerUpgradeTypes.HealPerHit, UpgradeTiers.x3).Apply();
        base.Apply();
    }
}