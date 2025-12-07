public class ResistUpgrade : PlayerUpgrade
{
    public ResistUpgrade(PlayerUpgradeController man, UpgradeTiers tier) : base(man, tier)
    {
    }

    public override void Apply()
    {
        UpgradeManager.PlayerModel.UpgradeResist(UpgradeValue);
    }
}