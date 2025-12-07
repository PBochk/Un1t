public class ShieldAbility : PlayerAbility
{
    public ShieldAbility(PlayerUpgradeController man) : base(man)
    {
    }

    public override void Apply()
    {
        UpgradeManager.PlayerModel.UnlockShield(UpgradeValue);
        base.Apply();
    }
}