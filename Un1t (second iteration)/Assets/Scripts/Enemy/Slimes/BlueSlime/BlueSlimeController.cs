using System;
using UnityEngine;

[RequireComponent(typeof(SlimeFollowState),
    typeof(DeadState),
    typeof(IdleState))]
[RequireComponent(typeof(BlueSlimeView),
    typeof(SlimeMeleeAttackState))]
public class BlueSlimeController : EnemyController
{
    private const float BASE_RANGE = 0.75f;
    private const float BASE_AGGRO_RANGE = 1f;
   
    private SlimeFollowState followState;
    private SlimeMeleeAttackState meleeState;

    private EnemyStateTransition entryExit;
    private EnemyStateTransition followExit;
    private EnemyStateTransition meleeExit;
   
    protected override void Awake()
    {
        base.Awake();
      
        //TODO: Make methods in base class for initialization steps
        //this includes: State binding, entry state
      
        followState = GetComponent<SlimeFollowState>();
        meleeState = GetComponent<SlimeMeleeAttackState>();
        View = GetComponent<BlueSlimeView>();

        MakeTransitions();
    }

    private void MakeTransitions()
    {
        entryExit = new UnconditionalTransition(this, followState);
        followExit = new ConditionalTransition(this, FollowExitCondition, meleeState, followState);
        meleeExit = new UnconditionalTransition(this, meleeState);
        
        IdleState.MakeTransition(entryExit);
        followState.MakeTransition(followExit);
        meleeState.MakeTransition(meleeExit);
    }

    private bool FollowExitCondition(IEnemyTarget target, EnemyModel model)
    {
        return Vector2.Distance(target.Position, Rb.position) <= BASE_AGGRO_RANGE;
    }
}