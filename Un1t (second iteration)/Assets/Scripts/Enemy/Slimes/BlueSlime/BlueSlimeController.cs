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
   
    [Header("States")]
    [SerializeField] private SlimeFollowState followState;
    [SerializeField] private SlimeMeleeAttackState meleeState;
    [SerializeField] private CooldownState afterJumpCooldown;
    [SerializeField] private CooldownState afterAttackCooldown;
    [SerializeField] private DecisionState decisionState;
    [SerializeField] private SpriteRenderer meleeAttack;

    [Header("Other Components And Objects")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private GameObject MeleeAttackHitbox;
    
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
        idleTransition = new UnconditionalTransition(this, decisionState);
        // =========================
        // DECISION STATES
        // =========================
        decisionTransition = new ConditionalTransition(
            this,
            CheckInRange,
            meleeState,    // If in range - attack
            followState    // Else - follow target
        );


        // =========================
        // ACTION STATES
        // =========================
        followTransition = new UnconditionalTransition(this, afterJumpCooldown);
        meleeTransition = new UnconditionalTransition(this, afterAttackCooldown);


        // =========================
        // COOLDOWN STATES
        // =========================
        afterJumpCooldownTransition = new UnconditionalTransition(this, decisionState);
        afterAttackCooldownTransition = new UnconditionalTransition(this, decisionState);


        // =========================
        // TRANSITION REGISTRATION
        // =========================
        IdleState.MakeTransition(idleTransition);
        followState.MakeTransition(followTransition);
        meleeState.MakeTransition(meleeTransition);
        decisionState.MakeTransition(decisionTransition);
        afterJumpCooldown.MakeTransition(afterJumpCooldownTransition);
        afterAttackCooldown.MakeTransition(afterAttackCooldownTransition);

    }

    protected override void TurnOffAllHitboxes()
    {
        //boxCollider.enabled = false;
        //MeleeAttackHitbox.SetActive(false);
    }
    private bool CheckInRange(EnemyTargetComponent target)
    {
        return Vector2.Distance(target.Position, Rb.position) <= ModelMB.Config.AggroRange;
    }
}