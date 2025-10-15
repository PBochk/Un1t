using UnityEngine;

public class SlimeMeleeAttackState : EnemyState
{
    public override void EnterState(IEnemyTarget target, EnemyModel model)
    {
        base.EnterState(target, model);
        ExitState();
    }
}