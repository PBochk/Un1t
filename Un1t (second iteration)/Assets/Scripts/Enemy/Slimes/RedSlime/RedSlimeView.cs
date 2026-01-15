using UnityEngine;

[RequireComponent(typeof(EnemySoundPlayer))]
public class RedSlimeView : EnemyView
{
    [Header("States")] [SerializeField] private SlimeFollowState followState;
    [SerializeField] private SlimeMeleeAttackState meleeState;
    [SerializeField] private SlimeRangedAttackState rangedAttackState;
    [SerializeField] private SlimeRunawayState runawayState;
    [SerializeField] private CooldownState followCooldownState;
    [SerializeField] private CooldownState meleeCooldownState;
    [SerializeField] private CooldownState rangedCooldownState;
    [SerializeField] private CooldownState runawayCooldownState;
    [SerializeField] private DeadState deadState;

    [Header("Animator")] [SerializeField] private SlimeAnimator animator;
    [Header("Children")] [SerializeField] private SpriteRenderer meleeHitboxDebug;

    protected override void BindStates()
    {
    }

    protected override void BindAnimator()
    {
        animator.PlayIdleAnimation();
        
        followState.OnStateEnter.AddListener(() =>
        {
            animator.AdjustJumpAnimationSpeed(followState.MotionTime);
            animator.PlayJumpAnimation();
        });
        
        followState.OnStateExit.AddListener(animator.PlayIdleAnimation);
        
        //followCooldownState.OnStateEnter.AddListener(animator.PlayIdleAnimation);
        

        runawayState.OnStateEnter.AddListener(() =>
        {
            animator.AdjustJumpAnimationSpeed(runawayState.MotionTime);
            animator.PlayJumpAnimation();
        });
        
        runawayState.OnStateExit.AddListener(animator.PlayIdleAnimation);

        //runawayCooldownState.OnStateExit.AddListener(animator.PlayIdleAnimation);
        
        meleeState.OnStateEnter.AddListener(() =>
        {
            animator.AdjustMeleeAttackSpeed(meleeState.MotionTime);
            animator.PlayMeleeAttackAnimation();
        });
        
        meleeState.OnStateExit.AddListener(animator.PlayIdleAnimation);

        //meleeCooldownState.OnStateEnter.AddListener(animator.PlayIdleAnimation);
        
        rangedAttackState.OnStateEnter.AddListener(() =>
        {
            animator.AdjustRangedAttackSpeed(rangedAttackState.MotionTime);
            animator.PlayRangedAttackAnimation();
        });
        
        rangedAttackState.OnStateExit.AddListener(animator.PlayIdleAnimation);
        
        //rangedCooldownState.OnStateEnter.AddListener(animator.PlayIdleAnimation);
        
        deadState.OnStateEnter.AddListener(animator.PlayDeathAnimation);
    }

    protected override void BindSoundPlayer()
    {
        soundPlayer = GetComponent<EnemySoundPlayer>();
        followState.OnStateEnter.AddListener(() =>
        {
            soundPlayer.PlayMoveSound();
        });
        runawayState.OnStateEnter.AddListener(() =>
        {
            soundPlayer.PlayMoveSound();
        });
        deadState.OnStateEnter.AddListener(() =>
        {
            soundPlayer.PlaydDeathSound();
        });
        meleeState.OnStateEnter.AddListener(() =>
        {
            soundPlayer.PlayAttackSound();
        });
        rangedAttackState.OnStateEnter.AddListener(() =>
        {
            soundPlayer.PlayAttackSound();
        });
    }

    public override void ResetAllAnimations()
    {
        animator.PlayIdleAnimation();
    }

    public void ShowDebugMeleeHitbox()
    {
        meleeHitboxDebug.enabled = true;
    }

    public void HideDebugMeleeHitbox()
    {
        meleeHitboxDebug.enabled = false;
    }
}