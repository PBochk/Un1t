using System;
using System.Collections.Generic;

public abstract class PlayerAbility : PlayerUpgrade
{
    /// <summary>
    /// Upgrade manager should subscribe on this and remove ability after apply
    /// </summary>
    public event Action AbilityApplied;
    public PlayerAbility(PlayerUpgradeController man) : base(man)
    {
    }
    public override void Apply()
    {
        AbilityApplied.Invoke();
    }
    public void RemoveListeners()
    {
        AbilityApplied = null;
    }
}