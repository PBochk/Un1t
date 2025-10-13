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
    private EnemyStateTransition idleTransition;
    private EnemyStateTransition followTransition;
    private EnemyStateTransition meleeTransition;
    private EnemyStateTransition decisionTransition;
    private EnemyStateTransition afterJumpCooldownTransition;
    private EnemyStateTransition afterAttackCooldownTransition;
   
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
        idleTransition = new UnconditionalTransition(this, followState);
        followTransition = new UnconditionalTransition(this, afterJumpCooldown);
        afterJumpCooldownTransition = new UnconditionalTransition(this, decisionState);
        decisionTransition = new ConditionalTransition(this, CheckInRange, meleeState, followState);
        meleeTransition = new UnconditionalTransition(this, afterAttackCooldown);
        afterAttackCooldownTransition = new UnconditionalTransition(this, decisionState);
        
        
        IdleState.MakeTransition(idleTransition);
        followState.MakeTransition(followTransition);
        meleeState.MakeTransition(meleeTransition);
        decisionState.MakeTransition(decisionTransition);
        meleeState.MakeTransition(meleeTransition);
        afterAttackCooldown.MakeTransition(afterAttackCooldownTransition);
        afterJumpCooldown.MakeTransition(afterJumpCooldownTransition);
    }

    private bool CheckInRange(IEnemyTarget target, EnemyModel model)
    {
        return Vector2.Distance(target.Position, Rb.position) <= BASE_AGGRO_RANGE;
    }
}