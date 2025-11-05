using System.Collections;
using UnityEngine;

public class SlimeMeleeAttackState : EnemyState
{
    public override float MotionTime => 0.88f;
    public override void EnterState(EnemyTargetComponent target)
    {
        base.EnterState(target);
        StartCoroutine(WaitForAttack());
    }

    private IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(MotionTime);
        ExitState();
    }
}