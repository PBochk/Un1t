using UnityEngine;

[RequireComponent(typeof(SlimeFollowState))]
[RequireComponent(typeof(DeadState))]
[RequireComponent(typeof(IdleState))]
[RequireComponent(typeof(GreenSlimeView))]
[RequireComponent(typeof(EnemyModelMB))]
[RequireComponent(typeof(SlimeRangedAttackState))]
[RequireComponent(typeof(SlimeRunawayState))]
[RequireComponent(typeof(DecisionState))]
public class GreenSlimeController : EnemyController
{
    [Header("States")]
    [SerializeField] private IdleState idleState;
    [SerializeField] private SlimeFollowState followState;
    [SerializeField] private SlimeRangedAttackState rangedAttackState;
    [SerializeField] private SlimeRunawayState runawayState;
    [SerializeField] private DecisionState isInRangeDecisionState;
    [SerializeField] private DecisionState isTooCloseDecisionState;
    [SerializeField] private CooldownState followCooldownState;
    [SerializeField] private CooldownState runawayCooldownState;
    [SerializeField] private CooldownState attackCooldownState;
    [SerializeField] private GreenSlimeView view;
    
    //TODO: fix this shit and make better config system
    [Header("Config that isn't in config")]
    [Tooltip("In units")]
    [SerializeField]
    private float tooCloseRange = 0.5f;

    private EnemyStateTransition idleTransition;
    private EnemyStateTransition followTransition;
    private EnemyStateTransition inRangeDecisionTransition;
    private EnemyStateTransition tooCloseDecisionTransition;
    private EnemyStateTransition runawayTransition;
    private EnemyStateTransition attackTransition;
    private EnemyStateTransition followCooldownTransition;
    private EnemyStateTransition attackCooldownTransition;
    private EnemyStateTransition runawayCooldownTransition;
    
    protected override void BindStates()
    {
    }

    protected override void BindView()
    {
    }

    protected override void MakeTransitions()
    {
        idleTransition = new UnconditionalTransition(this, isInRangeDecisionState);
        // =========================
        // DECISION STATES
        // =========================
        inRangeDecisionTransition = new ConditionalTransition(
            this,
            CheckTargetInRange,
            isTooCloseDecisionState,   // If target is in range check if it's too close
            followState                // Else follow target
        );

        tooCloseDecisionTransition = new ConditionalTransition(
            this,
            CheckTargetTooClose,
            runawayState,              // If too close - runaway
            rangedAttackState          // Else - attack
        );


        // =========================
        // ACTION STATES
        // =========================
        followTransition = new UnconditionalTransition(this, followCooldownState);
        runawayTransition = new UnconditionalTransition(this, runawayCooldownState);
        attackTransition = new UnconditionalTransition(this, attackCooldownState);


        // =========================
        // COOLDOWN STATES
        // =========================
        followCooldownTransition = new UnconditionalTransition(this, isInRangeDecisionState);
        runawayCooldownTransition = new UnconditionalTransition(this, isInRangeDecisionState);
        attackCooldownTransition = new UnconditionalTransition(this, isInRangeDecisionState);


        // =========================
        // TRANSITION REGISTRATION
        // =========================
        idleState.MakeTransition(idleTransition);
        isInRangeDecisionState.MakeTransition(inRangeDecisionTransition);
        isTooCloseDecisionState.MakeTransition(tooCloseDecisionTransition);

        followState.MakeTransition(followTransition);
        runawayState.MakeTransition(runawayTransition);
        rangedAttackState.MakeTransition(attackTransition);

        followCooldownState.MakeTransition(followCooldownTransition);
        runawayCooldownState.MakeTransition(runawayCooldownTransition);
        attackCooldownState.MakeTransition(attackCooldownTransition);
    }

    private bool CheckTargetInRange(EnemyTargetComponent target)
    {
        return Vector2.Distance(target.Position, Rb.position) <= ModelMB.Config.AggroRange;
    }
    
    private bool CheckTargetTooClose(EnemyTargetComponent target)
    {
        return Vector2.Distance(target.Position, Rb.position) <= tooCloseRange;
    }
}