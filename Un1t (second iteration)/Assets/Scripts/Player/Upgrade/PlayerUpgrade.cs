using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerUpgrade
{
    public readonly PlayerUpgradeController UpgradeManager;
    public string Description { get; protected set; }
    public float UpgradeValue { get; protected set; }
    protected UpgradeTiers Tier { get; set; }

    protected PlayerUpgrade(PlayerUpgradeController man, UpgradeTiers tier = UpgradeTiers.x1)
    {
        UpgradeManager = man;
        Tier = tier;
        var config = Resources.Load<PlayerUpgradeConfig>(GetType().ToString());
        UpgradeValue = config.UpgradeValues[(int)tier];
        Description = config.Description + $" {UpgradeValue}";
    }

    public abstract void Apply();
}