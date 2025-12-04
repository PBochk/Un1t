public class DodgeChanceAbility : PlayerAbility
{
    public DodgeChanceAbility(PlayerUpgradeController man) : base(man)
    {
    }

    public override void Apply()
    {
        UnlockUpgrade(PlayerUpgradeTypes.DodgeChance);
        UpgradeFactory.GetUpgrade(PlayerUpgradeTypes.DodgeChance, UpgradeTiers.x3).Apply(); // TODO: make the PlayerUpgradeTiers.Unlock tier instead of this
        base.Apply();
    }
}