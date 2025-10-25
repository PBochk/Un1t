using System.Collections;
using UnityEngine;

public class SlimeMeleeAttackState : EnemyState
{
    public override void EnterState(EnemyTargetComponent target)
    {
        base.EnterState(target);
        StartCoroutine(WaitForAttack());
    }

    private IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(0.5f);
        ExitState();
    }
}