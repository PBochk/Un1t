using System.Collections;
using UnityEngine;

public class CooldownState : EnemyState
{
    [SerializeField] private float cooldown;
    WaitForSeconds cooldownTimer;
    
    public override void EnterState(IEnemyTarget target)
    {
        base.EnterState(target);
        cooldownTimer = new WaitForSeconds(cooldown);
        StartCoroutine(CooldownTimer());
    }

    private IEnumerator CooldownTimer()
    {
        yield return cooldownTimer;
        ExitState();
    }
}