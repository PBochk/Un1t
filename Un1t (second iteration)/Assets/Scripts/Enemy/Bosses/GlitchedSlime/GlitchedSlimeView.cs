using System.Xml;
using UnityEngine;

[RequireComponent(typeof(SlimeAnimator))]
[RequireComponent(typeof(EnemySoundPlayer))]
[RequireComponent(typeof(Rigidbody2D))]
public class GlitchedSlimeView : EnemyView
{
    [SerializeField] private TelegraphedJumpState followState;
    [SerializeField] private SmartRunawayState runawayState;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private DeadState deadState;
    private SlimeAnimator animator;
    private EnemySoundPlayer soundPlayer;
    private Rigidbody2D rb;
    
    protected override void BindStates()
    {
        rb = GetComponent<Rigidbody2D>();
        deadState = GetComponent<DeadState>();
        deadState.OnStateEnter.AddListener(() =>
        {
            GameOverUI.Instance.OnBossDeath();
        });

    }

    protected override void BindAnimator()
    {
        animator = GetComponent<SlimeAnimator>();
        animator.PlayAppearAnimation();
        followState.OnStateEnter.AddListener(() =>
        {
            animator.PlayJumpAnimation();
            animator.AdjustJumpAnimationSpeed(followState.MotionTime);
            spriteRenderer.flipX = followState.Direction.x > 0;
        });
        followState.OnStateExit.AddListener(() =>
        {
            animator.PlayIdleAnimation();
        });
        runawayState.OnStateEnter.AddListener(() =>
        {
            animator.PlayJumpAnimation();
        });
        runawayState.OnStateExit.AddListener(() =>
        {
            animator.PlayIdleAnimation();
            animator.AdjustJumpAnimationSpeed(runawayState.MotionTime);
            spriteRenderer.flipX = runawayState.direction.x > 0;
        });
        deadState.OnStateEnter.AddListener(() =>
        {
            animator.PlayDeathAnimation();
        });
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
    }

    public override void ResetAllAnimations()
    {
        animator.PlayIdleAnimation();
        animator.ResetAllTriggers();
    }
}