public class RegenerationAbility : PlayerAbility
{
    public RegenerationAbility(PlayerUpgradeController man) : base(man)
    {
    }

    public override void Apply()
    {
        UpgradeManager.PlayerModel.UpgradeRegeneration(UpgradeValue);
        base.Apply();
    }
}