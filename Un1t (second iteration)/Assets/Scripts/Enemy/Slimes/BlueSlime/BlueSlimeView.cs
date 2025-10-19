using UnityEngine;

public class BlueSlimeView : EnemyView
{
    [SerializeField] private SlimeFollowState followState;
    [SerializeField] private SlimeMeleeAttackState meleeState;
    [SerializeField] private CooldownState afterJumpCooldown;
    [SerializeField] private CooldownState afterAttackCooldown;
    [SerializeField] private DecisionState decisionState;
    [SerializeField] private SlimeAnimator animator;

    protected override void BindModel()
    {
    }

    protected override void BindStates()
    {
    }

    protected override void BindAnimator()
    {
        followState.OnStateEnter.AddListener(animator.PlayJumpAnimation);
        followState.OnStateExit.AddListener(animator.PlayIdleAnimation);
    }

    protected override void BindSoundPlayer()
    {
    }
}