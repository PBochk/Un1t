using UnityEngine;

[RequireComponent(typeof(EnemySoundPlayer))]
public class BlueSlimeView : EnemyView
{
    [SerializeField] private SlimeFollowState followState;
    [SerializeField] private SlimeMeleeAttackState meleeState;
    [SerializeField] private CooldownState afterJumpCooldown;
    [SerializeField] private CooldownState afterAttackCooldown;
    [SerializeField] private DecisionState decisionState;
    [SerializeField] private DeadState deadState;
    [SerializeField] private SlimeAnimator animator;

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
        
        meleeState.OnStateEnter.AddListener(() =>
        {
            animator.AdjustMeleeAttackSpeed(meleeState.MotionTime);
            animator.PlayMeleeAttackAnimation();
        });
        afterJumpCooldown.OnStateEnter.AddListener(() =>
        {
            animator.PlayIdleAnimation();
        });
        afterAttackCooldown.OnStateEnter.AddListener( () =>
        {
            animator.PlayIdleAnimation();
        });
        
        deadState.OnStateEnter.AddListener(animator.PlayDeathAnimation);
    }

    protected override void BindSoundPlayer()
    {
        soundPlayer = GetComponent<EnemySoundPlayer>();
        followState.OnStateEnter.AddListener(() =>
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
    }

    public override void ResetAllAnimations()
    {
    }
}