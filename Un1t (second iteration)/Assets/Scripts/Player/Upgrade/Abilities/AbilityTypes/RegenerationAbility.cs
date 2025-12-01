public class RegenerationAbility : PlayerAbility
{
    public RegenerationAbility(PlayerUpgradeManager man) : base(man)
    {
    }

    public override void Apply()
    {
        UpgradeManager.PlayerModel.UpgradeRegeneration(UpgradeValue);
        base.Apply();
    }
}