using System;
using UnityEngine;

//RequireComponent is still relevant because it satisfyingly adds all the components when adding controller
[RequireComponent(typeof(SlimeFollowState))]
[RequireComponent(typeof(DeadState))]
[RequireComponent(typeof(IdleState))]
[RequireComponent(typeof(BlueSlimeView))]
[RequireComponent(typeof(SlimeMeleeAttackState))]
[RequireComponent(typeof(EnemyModelMB))]
public class BlueSlimeController : EnemyController
{
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

    private bool CheckInRange(IEnemyTarget target)
    {
        return Vector2.Distance(target.Position, Rb.position) <= ModelMB.Config.AggroRange * 1.5f;
    }
}