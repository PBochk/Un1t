using UnityEngine;

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
        
        followCooldownState.OnStateEnter.AddListener(animator.PlayIdleAnimation);
        

        runawayState.OnStateEnter.AddListener(() =>
        {
            animator.AdjustJumpAnimationSpeed(runawayState.MotionTime);
            animator.PlayJumpAnimation();
        });

        runawayCooldownState.OnStateEnter.AddListener(animator.PlayIdleAnimation);
        
        meleeState.OnStateEnter.AddListener(() =>
        {
            animator.AdjustMeleeAttackSpeed(meleeState.MotionTime);
            animator.PlayMeleeAttackAnimation();
        });

        meleeCooldownState.OnStateEnter.AddListener(animator.PlayIdleAnimation);
        
        rangedAttackState.OnStateEnter.AddListener(() =>
        {
            animator.AdjustRangedAttackSpeed(rangedAttackState.MotionTime);
            animator.PlayRangedAttackAnimation();
        });
        
        rangedCooldownState.OnStateEnter.AddListener(animator.PlayIdleAnimation);
        
        deadState.OnStateEnter.AddListener(animator.PlayDeathAnimation);
    }

    protected override void BindSoundPlayer()
    {
        //throw new System.NotImplementedException();
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