using System;
using System.Collections.Generic;

public abstract class PlayerAbility : PlayerUpgrade
{
    /// <summary>
    /// Upgrade manager should subscribe on this and remove ability after apply
    /// </summary>
    public event Action<PlayerAbility> Remove;
    public PlayerAbility(PlayerUpgradeManager man) : base(man)
    {
    }
    public override void Apply()
    {
        Remove.Invoke(this);
    }
}