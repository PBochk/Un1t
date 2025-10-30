using UnityEngine;

[RequireComponent(typeof(SlimeFollowState))]
[RequireComponent(typeof(DeadState))]
[RequireComponent(typeof(IdleState))]
[RequireComponent(typeof(RedSlimeView))]
[RequireComponent(typeof(EnemyModelMB))]
[RequireComponent(typeof(SlimeRangedAttackState))]
[RequireComponent(typeof(SlimeMeleeAttackState))]
[RequireComponent(typeof(SlimeRunawayState))]
[RequireComponent(typeof(DecisionState))]
public class RedSlimeController : EnemyController
{
    protected override void BindStates()
    {
        throw new System.NotImplementedException();
    }

    protected override void BindView()
    {
        throw new System.NotImplementedException();
    }

    protected override void MakeTransitions()
    {
        throw new System.NotImplementedException();
    }
}