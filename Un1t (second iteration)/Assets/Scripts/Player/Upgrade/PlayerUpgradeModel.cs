using System.Collections.Generic;

public class PlayerUpgradeModel
{
    private List<PlayerUpgradeTypes> availableUpgrades;
    private List<PlayerAbilityTypes> availableAbilities;

    public List<PlayerUpgradeTypes> AvailableUpgrades => availableUpgrades;
    public List<PlayerAbilityTypes> AvailableAbilities => availableAbilities;
    public PlayerUpgradeModel(List<PlayerUpgradeTypes> availableUpgrades,
                              List<PlayerAbilityTypes> availableAbilities)
    {
        this.availableUpgrades = availableUpgrades;
        this.availableAbilities = availableAbilities;
    }

    public void RemoveAbility(PlayerAbilityTypes type)
    {
        availableAbilities.Remove(type);
    }
}