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
    
    [Header("States")]
    [SerializeField] private IdleState idleState;
    [SerializeField] private SlimeFollowState followState;
    [SerializeField] private SlimeMeleeAttackState meleeAttackState;
    [SerializeField] private SlimeRangedAttackState rangedAttackState;
    [SerializeField] private SlimeRunawayState runawayState;
    [SerializeField] private DecisionState isInRangeDecisionState;
    [SerializeField] private DecisionState isTooCloseDecisionState;
    [SerializeField] private CooldownState followCooldownState;
    [SerializeField] private CooldownState runawayCooldownState;
    [SerializeField] private CooldownState meleeCooldownState;
    [SerializeField] private CooldownState rangedCooldownState;
    
    [Header("View")]
    [SerializeField] private RedSlimeView view;

    [Header("Other gameobjects")] [SerializeField]
    private SingleShotWeapon weapon;
    
    //TODO: fix this shit and make better config system
    [Header("Config that isn't in config")]
    [Tooltip("In units")] [SerializeField]
    private float rangedRange;
    [Tooltip("In units")] [SerializeField]
    private float tooCloseRange;

    public void Shot()
    {
        weapon.Shot(Target);
    }
    
    protected override void BindStates()
    {
        ModelMB.OnDamageTaken.AddListener(CheckPhaseChange);
    }

    protected override void BindView()
    {
    }

    protected override void MakeTransitions()
    {
        EnterPhase1();
    }
    
    private void EnterPhase1()
    {        
        var idleTransition = new UnconditionalTransition(this, followState);
        // =========================
        // DECISION STATES
        // =========================
        var decisionTransition = new ConditionalTransition(
            this,
            CheckInMeleeRange,
            meleeAttackState,    // If in range - attack
            followState    // Else - follow target
        );


        // =========================
        // ACTION STATES
        // =========================
        var followTransition = new UnconditionalTransition(this, followCooldownState);
        var meleeTransition = new UnconditionalTransition(this, meleeCooldownState);


        // =========================
        // COOLDOWN STATES
        // =========================
        var followCooldownTransition = new UnconditionalTransition(this, isInRangeDecisionState);
        var meleeCooldownTransition = new UnconditionalTransition(this, isInRangeDecisionState);


        // =========================
        // TRANSITION REGISTRATION
        // =========================
        IdleState.MakeTransition(idleTransition);
        followState.MakeTransition(followTransition);
        meleeAttackState.MakeTransition(meleeTransition);
        isInRangeDecisionState.MakeTransition(decisionTransition);
        followCooldownState.MakeTransition(followCooldownTransition);
        meleeCooldownState.MakeTransition(meleeCooldownTransition);
    }

    private void EnterPhase2()
    {
        var idleTransition = new UnconditionalTransition(this, isInRangeDecisionState);
        // =========================
        // DECISION STATES
        // =========================
        var inRangeDecisionTransition = new ConditionalTransition(
            this,
            CheckInRangedRange,
            isTooCloseDecisionState,   // If target is in range check if it's too close
            followState                // Else follow target
        );

        var tooCloseDecisionTransition = new ConditionalTransition(
            this,
            CheckTooClose,
            runawayState,              // If too close - runaway
            rangedAttackState          // Else - attack
        );


        // =========================
        // ACTION STATES
        // =========================
        var followTransition = new UnconditionalTransition(this, followCooldownState);
        var runawayTransition = new UnconditionalTransition(this, runawayCooldownState);
        var attackTransition = new UnconditionalTransition(this, rangedCooldownState);


        // =========================
        // COOLDOWN STATES
        // =========================
        var followCooldownTransition = new UnconditionalTransition(this, isInRangeDecisionState);
        var runawayCooldownTransition = new UnconditionalTransition(this, isInRangeDecisionState);
        var rangedCooldownTransition = new UnconditionalTransition(this, isInRangeDecisionState);


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
        rangedCooldownState.MakeTransition(rangedCooldownTransition);
        
        //Reset state
        CurrentState.StopAllCoroutines();
        view.ResetAllAnimations();
        ChangeState(idleState);
    }
    
    protected override void TurnOffAllHitboxes()
    {
    }

    private bool CheckInMeleeRange(EnemyTargetComponent target)
    {
        return Vector2.Distance(target.Position, Rb.position) <= ModelMB.Config.AggroRange;
    }

    private bool CheckInRangedRange(EnemyTargetComponent target)
    {
        return Vector2.Distance(target.Position, Rb.position) <= rangedRange;
    }

    private bool CheckTooClose(EnemyTargetComponent target)
    {
        return Vector2.Distance(target.Position, Rb.position) <= tooCloseRange;
    }

    private void CheckPhaseChange()
    {
        if (ModelMB.NativeModel.Hp <= Model.Config.MaxHealth / 2)
        {
            EnterPhase2();
        }
    }
}