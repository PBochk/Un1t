using UnityEngine;

[RequireComponent(typeof(DeadState))]
[RequireComponent(typeof(Animator))]
public class GlitchView : EnemyView
{
    private DeadState deadState;
    private Animator animator;
    protected override void BindStates()
    {
        DeadState deadState = GetComponent<DeadState>();
        Animator animator = GetComponent<Animator>();
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
    }

    public override void ResetAllAnimations()
    {
    }
}