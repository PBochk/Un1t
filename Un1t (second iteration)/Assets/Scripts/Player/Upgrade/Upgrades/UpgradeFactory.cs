using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
public static class UpgradeFactory
{
    public static PlayerUpgradeController Manager;
    private static Dictionary<PlayerUpgradeTypes, Func<PlayerUpgrade>> UpgradeFactories = new()
    {
        { PlayerUpgradeTypes.MaxHealth, () => new MaxHealthUpgrade(Manager, GetRandomTier()) },
        { PlayerUpgradeTypes.HealCost, () => new HealCostUpgrade(Manager, GetRandomTier()) },
        { PlayerUpgradeTypes.XPGain, () => new XPGainUpgrade(Manager, GetRandomTier()) },
        { PlayerUpgradeTypes.MovingSpeed, () => new MovingSpeedUpgrade(Manager, GetRandomTier()) },
        { PlayerUpgradeTypes.MeleeSpeed, () => new MeleeAttackSpeedUpgrade(Manager, GetRandomTier()) },
        { PlayerUpgradeTypes.MeleeDamage, () => new MeleeDamageUpgrade(Manager, GetRandomTier()) },
        { PlayerUpgradeTypes.RangeDamage, () => new RangeDamageUpgrade(Manager, GetRandomTier()) }
    };

    public static PlayerUpgrade GetUpgrade(PlayerUpgradeTypes type)
    {
        return UpgradeFactories[type]();
    }
    private static UpgradeTiers GetRandomTier()
    {
        // TODO: replace magic numbers
        var rand = Random.Range(1, 11);
        if (rand <= 6) return UpgradeTiers.x1;
        else if (rand <= 9) return UpgradeTiers.x2;
        else return UpgradeTiers.x3;
    }
}