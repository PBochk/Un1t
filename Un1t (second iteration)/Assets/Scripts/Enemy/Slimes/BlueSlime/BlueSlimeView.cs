using UnityEngine;

public class BlueSlimeView : EnemyView
{
    [SerializeField] private SlimeFollowState followState;
    [SerializeField] private SlimeMeleeAttackState meleeState;
    [SerializeField] private CooldownState afterJumpCooldown;
    [SerializeField] private CooldownState afterAttackCooldown;
    [SerializeField] private DecisionState decisionState;
    [SerializeField] private SlimeAnimator animator;

    //TODO: Decide how to get model from view and if we should even use it
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