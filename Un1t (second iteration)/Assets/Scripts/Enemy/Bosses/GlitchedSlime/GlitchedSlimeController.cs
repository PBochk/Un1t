using UnityEngine;

[RequireComponent(typeof(DeadState))]
[RequireComponent(typeof(IdleState))]
[RequireComponent(typeof(EnemyModelMB))]
[RequireComponent(typeof(TelegraphedJumpState))]
[RequireComponent(typeof(SlimeRangedAttackState))]
[RequireComponent(typeof(DecisionState))]
public class GlitchedSlimeController : EnemyController
{
    [Header("States")]
    [SerializeField] private IdleState idleState;
    [SerializeField] private TelegraphedJumpState followState;
    [SerializeField] private SmartRunawayState runawayState;
    [SerializeField] private SlimeRangedAttackState rangedAttackState;
    [SerializeField] private DecisionState phaseTransitionState;
    [SerializeField] private DecisionState isInRangeDecisionState;
    [SerializeField] private DecisionState isTooCloseDecisionState;
    
    [Header("Cooldowns")]
    [SerializeField] private CooldownState followCooldownState;
    [SerializeField] private CooldownState runawayCooldownState;
    [SerializeField] private CooldownState summonCooldownState;

    [Header("Weapons")]
    [SerializeField] private SummonWeapon weapon;
    [SerializeField] private BoxCollider2D boxCollider2D;
    
    [Header("View")]
    [SerializeField] private GlitchedSlimeView view;
    //[SerializeField] private GameObject meleeHitbox;
    
    [Header("Config that isn't in config")]
    [Tooltip("In units")] [SerializeField]
    private float rangedRange;
    [Tooltip("In units")] [SerializeField]
    private float tooCloseRange;

    private int runawayCounter;

    public void Shot()
    {
        weapon.Shot(Target);
    }
    
    protected override void BindStates()
    {
        //ModelMB.OnDamageTaken.AddListener(CheckPhaseChange);
        ModelMB.OnDamageTaken.AddListener(() =>
        {
            Debug.Log($"Boss took damage. Hp ({ModelMB.NativeModel.Hp})/{ModelMB.Config.MaxHealth}");
        });
        phaseTransitionState.OnStateEnter.AddListener(() =>
        {
            EnterPhase2();
            ResetState();
        });
        rangedAttackState.OnStateEnter.AddListener(Shot);
        rangedAttackState.OnStateEnter.AddListener(() => runawayCounter = 0);
        runawayState.OnStateEnter.AddListener(() => runawayCounter++);
    }
    
    private void ResetState()
    {
        //Reset state
        CurrentState.StopAllCoroutines();
        //view.ResetAllAnimations();
        ChangeState(idleState);
    }

    protected override void BindView()
    {
    }

    protected override void MakeTransitions()
    {
        EnterPhase2();
    }

    
    
    private void EnterPhase1()
    {        
        var idleTransition = new UnconditionalTransition(this, followState);

        // =========================
        // ACTION STATES
        // =========================
        var followTransition = new UnconditionalTransition(this, followCooldownState);


        // =========================
        // COOLDOWN STATES
        // =========================
        var followCooldownTransition = new ConditionalTransition(
            this,
            _ => CheckPhaseChange(),
            phaseTransitionState,
            followState
        );


        // =========================
        // TRANSITION REGISTRATION
        // =========================
        IdleState.MakeTransition(idleTransition);
        followState.MakeTransition(followTransition);
        followCooldownState.MakeTransition(followCooldownTransition);
    }

    private void EnterPhase2()
    {
        Debug.Log("Boss entering Phase2");

        // =========================
        // DECISION STATES
        // =========================
        var idleTransition = new ConditionalTransition(
            this,
            target => CheckTooClose(target) && runawayCounter < 2,
            runawayState,       
            rangedAttackState   
        );


        // =========================
        // ACTION STATES
        // =========================
        var attackTransition = new UnconditionalTransition(this, summonCooldownState);

        var runawayTransition = new UnconditionalTransition(this, runawayCooldownState);


        // =========================
        // COOLDOWN STATES
        // =========================
        var rangedCooldownTransition = new UnconditionalTransition(this, idleState);

        var runawayCooldownTransition = new UnconditionalTransition(this, idleState);


        // =========================
        // TRANSITION REGISTRATION
        // =========================
        idleState.MakeTransition(idleTransition);

        rangedAttackState.MakeTransition(attackTransition);
        runawayState.MakeTransition(runawayTransition);

        summonCooldownState.MakeTransition(rangedCooldownTransition);
        runawayCooldownState.MakeTransition(runawayCooldownTransition);
    }
    
    
    
    protected override void TurnOffAllHitboxes()
    {
        boxCollider2D.enabled = false;
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

    private bool CheckPhaseChange()
    {
        return ModelMB.NativeModel.Hp <= Model.Config.MaxHealth / 2;
    }
}