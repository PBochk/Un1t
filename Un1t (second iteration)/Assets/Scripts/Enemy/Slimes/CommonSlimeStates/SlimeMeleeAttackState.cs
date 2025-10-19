using UnityEngine;

public class SlimeMeleeAttackState : EnemyState
{
    public override void EnterState(IEnemyTarget target)
    {
        base.EnterState(target);
        ExitState();
    }
}