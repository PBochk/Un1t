public class HealCostAbility : PlayerAbility
{
    public HealCostAbility(PlayerUpgradeController man) : base(man)
    {
    }

    public override void Apply()
    {
        UnlockUpgrade(PlayerUpgradeTypes.HealCost);
        UpgradeFactory.GetUpgrade(PlayerUpgradeTypes.HealCost, UpgradeTiers.x3).Apply();
        base.Apply();
    }
}