using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerUpgrade
{
    public readonly PlayerUpgradeController UpgradeManager;
    public Sprite Icon { get; protected set; }
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public float UpgradeValue { get; protected set; }
    protected UpgradeTiers Tier { get; set; }

    protected PlayerUpgrade(PlayerUpgradeController man, UpgradeTiers tier = UpgradeTiers.x1)
    {
        UpgradeManager = man;
        Tier = tier;
        var config = Resources.Load<PlayerUpgradeConfig>(GetType().ToString());
        if (config.UpgradeValues.Length != 0)
        {
            UpgradeValue = config.UpgradeValues[(int)tier];
        }
        Icon = config.Icon;
        Name = config.Name;
        Description = config.Description;
        if (UpgradeValue != 0)
        {
            ParseDescription();
        }
    }

    protected void ParseDescription(string valueToPaste = null)
    {
        valueToPaste ??= (UpgradeValue > 1) ? UpgradeValue.ToString() : UpgradeValue * 100 + "%";
        Description = Description.Replace("{UpgradeValue}", valueToPaste);
    }

    public abstract void Apply();
}