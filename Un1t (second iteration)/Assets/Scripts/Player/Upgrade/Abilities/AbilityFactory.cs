using System;
using System.Collections.Generic;
public static class AbilityFactory
{
    public static PlayerUpgradeController Manager;
    private static Dictionary<PlayerAbilityTypes, Func<PlayerAbility>> AbilityFactories = new()
    {
        { PlayerAbilityTypes.Regeneration, () => new RegenerationAbility(Manager) },
        { PlayerAbilityTypes.XPGain, () => new XPGainAbility(Manager) },
    };

    public static PlayerAbility GetAbility(PlayerAbilityTypes type)
    {
        return AbilityFactories[type]();
    }
}