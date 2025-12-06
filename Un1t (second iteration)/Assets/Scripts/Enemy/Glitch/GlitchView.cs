using UnityEngine;

[RequireComponent(typeof(DeadState))]
[RequireComponent(typeof(GlitchDashState))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemySoundPlayer))]
public class GlitchView : EnemyView
{
    private DeadState deadState;
    private Animator animator;
    private GlitchDashState dashState;
    
    protected override void BindStates()
    {
        dashState = GetComponent<GlitchDashState>();
        deadState = GetComponent<DeadState>();
        animator = GetComponent<Animator>();
        deadState.OnStateEnter.AddListener(() =>
        {
            animator.SetTrigger("Dead");
        });
    }

    protected override void BindAnimator()
    {
    }

    protected override void BindSoundPlayer()
    {
        soundPlayer = GetComponent<EnemySoundPlayer>();
        dashState.OnStateEnter.AddListener(() =>
        {
            soundPlayer.PlayAttackSound();
        });
        deadState.OnStateEnter.AddListener(() =>
        {
            soundPlayer.PlaydDeathSound();
        });
    }

    public override void ResetAllAnimations()
    {
    }
}