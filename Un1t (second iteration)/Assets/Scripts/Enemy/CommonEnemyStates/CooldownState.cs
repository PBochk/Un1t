using System.Collections;
using UnityEngine;

public class CooldownState : EnemyState
{
    [SerializeField] private float cooldown;
    WaitForSeconds cooldownTimer;
    
    public override void EnterState(IEnemyTarget target, EnemyModel model)
    {
        base.EnterState(target, model);
        cooldownTimer = new WaitForSeconds(cooldown);
    }

    private IEnumerator CooldownTimer()
    {
        yield return cooldownTimer;
        ExitState();
    }
}