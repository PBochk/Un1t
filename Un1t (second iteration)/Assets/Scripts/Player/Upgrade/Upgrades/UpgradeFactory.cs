using System;
using System.Collections.Generic;
using System.Threading;
using Random = UnityEngine.Random;
public static class UpgradeFactory
{
    public static PlayerUpgradeController Manager;
    private static Dictionary<PlayerUpgradeTypes, Func<UpgradeTiers, PlayerUpgrade>> UpgradeFactories = new()
    {
        { PlayerUpgradeTypes.MaxHealth, (tier) => new MaxHealthUpgrade(Manager, tier) },
        { PlayerUpgradeTypes.HealCost, (tier) => new HealCostUpgrade(Manager, tier) },
        { PlayerUpgradeTypes.HealPerHit, (tier) => new HealPerHitUpgrade(Manager, tier) },
        { PlayerUpgradeTypes.XPGain, (tier) => new XPGainUpgrade(Manager, tier) },
        { PlayerUpgradeTypes.Resist, (tier) => new ResistUpgrade(Manager, tier) },
        { PlayerUpgradeTypes.MovingSpeed, (tier) => new MovingSpeedUpgrade(Manager, tier) },
        { PlayerUpgradeTypes.MeleeSpeed, (tier) => new MeleeAttackSpeedUpgrade(Manager, tier) },
        { PlayerUpgradeTypes.MeleeDamage, (tier) => new MeleeDamageUpgrade(Manager, tier) },
        { PlayerUpgradeTypes.RangeDamage, (tier) => new RangeDamageUpgrade(Manager, tier) },
        { PlayerUpgradeTypes.DoubleHitChance, (tier) => new DoubleHitChanceUpgrade(Manager, tier) },
    };

    public static PlayerUpgrade GetUpgrade(PlayerUpgradeTypes type, UpgradeTiers tier = UpgradeTiers.Random)
    {
        if (tier == UpgradeTiers.Random)
        {
            tier = GetRandomTier();
        }
        return UpgradeFactories[type](tier);
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