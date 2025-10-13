using System;
using UnityEngine;

//RequireComponent is still relevant because it satisfyingly adds all the components when adding controller
[RequireComponent(typeof(SlimeFollowState),
    typeof(DeadState),
    typeof(IdleState))]
[RequireComponent(typeof(BlueSlimeView),
    typeof(SlimeMeleeAttackState))]
public class BlueSlimeController : EnemyController
{
    //TODO: Make this configurable
    private const float BASE_RANGE = 0.75f;
    private const float BASE_AGGRO_RANGE = 1f;

    private BlueSlimeView view;
   
    [SerializeField] private SlimeFollowState followState;
    [SerializeField] private SlimeMeleeAttackState meleeState;
    [SerializeField] private CooldownState afterJumpCooldown;
    [SerializeField] private CooldownState afterAttackCooldown;
    [SerializeField] private DecisionState decisionState;

    //TODO: Consider to remove
    private EnemyStateTransition entryExit;
    private EnemyStateTransition followExit;
    private EnemyStateTransition meleeExit;
    private EnemyStateTransition decisionExit;
    private EnemyStateTransition afterJumpCooldownExit;
    private EnemyStateTransition afterAttackCooldownExit;
   
    protected override void BindModel()
    {
    }

    protected override void BindStates()
    {
    }

    protected override void BindView()
    {
        view = GetComponent<BlueSlimeView>();
    }

    protected override void MakeTransitions()
    {
        entryExit = new UnconditionalTransition(this, followState);
        followExit = new UnconditionalTransition(this, afterJumpCooldown);
        afterJumpCooldownExit = new UnconditionalTransition(this, decisionState);
        decisionExit = new ConditionalTransition(this, CheckInRange, meleeState, followState);
        meleeExit = new UnconditionalTransition(this, afterAttackCooldown);
        afterAttackCooldownExit = new UnconditionalTransition(this, decisionState);
        
        
        IdleState.MakeTransition(entryExit);
        followState.MakeTransition(followExit);
        meleeState.MakeTransition(meleeExit);
        decisionState.MakeTransition(decisionExit);
        meleeState.MakeTransition(meleeExit);
        afterAttackCooldown.MakeTransition(afterAttackCooldownExit);
        afterJumpCooldown.MakeTransition(afterJumpCooldownExit);
    }

    private bool CheckInRange(IEnemyTarget target, EnemyModel model)
    {
        return Vector2.Distance(target.Position, Rb.position) <= BASE_AGGRO_RANGE;
    }
}