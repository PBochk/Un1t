using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerUpgrade
{
    public readonly UpgradeManager UpgradeManager;
    public string Description { get; protected set; }
    public float UpgradeValue { get; protected set; }
    public UpgradeTiers Tier { get; protected set; }

    protected PlayerUpgrade(UpgradeManager man, UpgradeTiers tier)
    {
        UpgradeManager = man;
        Tier = tier;
        var config = Resources.Load<PlayerUpgradeConfig>(GetType().ToString());
        UpgradeValue = config.UpgradeValues[(int)tier];
        Description = config.Description + $" {UpgradeValue}";
    }

    public abstract void Apply();
}