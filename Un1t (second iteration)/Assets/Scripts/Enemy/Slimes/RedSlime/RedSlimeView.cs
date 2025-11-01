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
            var speed = 1 * model.NativeModel.SpeedCoeff / followState.baseMoveTime;
            animator.SetPlaybackSpeed(speed);
            animator.PlayJumpAnimation();
        });
        followState.OnStateExit.AddListener(() =>
        {
            animator.ResetPlaybackSpeed();
            animator.PlayIdleAnimation();
        });

        runawayState.OnStateEnter.AddListener(() =>
        {
            var speed = 1 * model.NativeModel.SpeedCoeff / runawayState.baseMoveTime;
            animator.SetPlaybackSpeed(speed);
            animator.PlayJumpAnimation();
        });
        runawayState.OnStateExit.AddListener(() =>
        {
            animator.ResetPlaybackSpeed();
            animator.PlayIdleAnimation();
        });

        meleeState.OnStateEnter.AddListener(animator.PlayMeleeAttackAnimation);
        meleeState.OnStateExit.AddListener(animator.PlayIdleAnimation);

        rangedAttackState.OnStateEnter.AddListener(animator.PlayRangedAttackAnimation);
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