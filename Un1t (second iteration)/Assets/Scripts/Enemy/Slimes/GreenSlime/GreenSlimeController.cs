using UnityEngine;

[RequireComponent(typeof(SlimeFollowState))]
[RequireComponent(typeof(DeadState))]
[RequireComponent(typeof(IdleState))]
[RequireComponent(typeof(BlueSlimeView))]
[RequireComponent(typeof(EnemyModelMB))]
[RequireComponent(typeof(SlimeRangedAttackState))]
[RequireComponent(typeof(SlimeRunawayState))]
public class GreenSlimeController : EnemyController
{
    protected override void BindStates()
    {
        //throw new System.NotImplementedException();
    }

    protected override void BindView()
    {
        //throw new System.NotImplementedException();
    }

    protected override void MakeTransitions()
    {
        //throw new System.NotImplementedException();
    }
}