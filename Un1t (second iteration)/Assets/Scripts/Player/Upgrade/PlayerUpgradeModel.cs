using System;
using System.Collections.Generic;

public class PlayerUpgradeModel
{
    private List<PlayerUpgradeTypes> unlockedUpgrades;
    private List<PlayerAbilityTypes> availableAbilities;

    public List<PlayerUpgradeTypes> UnlockedUpgrades => unlockedUpgrades;
    public List<PlayerAbilityTypes> AvailableAbilities => availableAbilities;

    // will be needed for upgrades revert 
    //public Stack<PlayerUpgrade> ReceivedUpgrades { get; private set; }

    public PlayerUpgradeModel(List<PlayerUpgradeTypes> unlockedUpgrades,
                              List<PlayerAbilityTypes> availableAbilities)
    {
        this.unlockedUpgrades = unlockedUpgrades;
        this.availableAbilities = availableAbilities;
    }

    public void RemoveAbility(PlayerAbilityTypes type)
    {
        availableAbilities.Remove(type);
    }

    public void UnlockUpgrade(PlayerUpgradeTypes type)
    {
        if (unlockedUpgrades.Contains(type))
        {
            throw new Exception("This ability is already unlocked");
        }
        unlockedUpgrades.Add(type);
    }
}