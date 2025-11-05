using UnityEngine;

public class RedSlimeView : EnemyView
{
    [Header("States")] [SerializeField] private SlimeFollowState followState;
    [SerializeField] private SlimeMeleeAttackState meleeState;
    [SerializeField] private SlimeRangedAttackState rangedAttackState;
    [SerializeField] private SlimeRunawayState runawayState;

    [Header("Animator")] [SerializeField] private SlimeAnimator animator;
    [Header("Children")] [SerializeField] private SpriteRenderer meleeHitboxDebug;

    protected override void BindStates()
    {
        followState.OnStateEnter.AddListener(() =>
        {
            animator.AdjustJumpAnimationSpeed(followState.MotionTime);
            animator.PlayJumpAnimation();
        });
        followState.OnStateExit.AddListener(() =>
        {
            animator.PlayIdleAnimation();
        });

        runawayState.OnStateEnter.AddListener(() =>
        {
            animator.AdjustJumpAnimationSpeed(runawayState.MotionTime);
            animator.PlayJumpAnimation();
        });
        runawayState.OnStateExit.AddListener(() =>
        {
            animator.PlayIdleAnimation();
        });

        meleeState.OnStateEnter.AddListener(() =>
        {
            animator.AdjustMeleeAttackSpeed(meleeState.MotionTime);
            animator.PlayMeleeAttackAnimation();
        });
        meleeState.OnStateExit.AddListener(animator.PlayIdleAnimation);

        rangedAttackState.OnStateEnter.AddListener(() =>
        {
            animator.AdjustRangedAttackSpeed(rangedAttackState.MotionTime);
            animator.PlayRangedAttackAnimation();
        });
        rangedAttackState.OnStateExit.AddListener(animator.PlayIdleAnimation);
    }

    protected override void BindAnimator()
    {
        //throw new System.NotImplementedException();
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