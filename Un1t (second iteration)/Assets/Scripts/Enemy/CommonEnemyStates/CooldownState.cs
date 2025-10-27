using System.Collections;
using UnityEngine;

public class CooldownState : EnemyState
{
    [Tooltip("Usually enemies can have multiple different cooldowns, so this is made to differentiate them in inspector")]
    [SerializeField] private string label;
    [SerializeField] private float cooldown;
    WaitForSeconds cooldownTimer;
    
    public override void EnterState(EnemyTargetComponent target)
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