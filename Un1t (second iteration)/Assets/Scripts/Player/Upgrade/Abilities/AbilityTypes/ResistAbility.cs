public class ResistAbility : PlayerAbility
{
    public ResistAbility(PlayerUpgradeController man) : base(man)
    {
    }

    public override void Apply()
    {
        UnlockUpgrade(PlayerUpgradeTypes.Resist);
        UpgradeFactory.GetUpgrade(PlayerUpgradeTypes.Resist, UpgradeTiers.x3).Apply();
        base.Apply();
    }
}