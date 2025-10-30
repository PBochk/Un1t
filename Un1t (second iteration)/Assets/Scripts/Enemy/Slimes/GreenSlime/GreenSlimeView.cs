using UnityEngine;

[RequireComponent(typeof(SlimeAnimator))]
public class GreenSlimeView : EnemyView
{
    [SerializeField] private IdleState idleState;
    [SerializeField] private SlimeFollowState followState;
    [SerializeField] private SlimeRangedAttackState rangedAttackState;
    [SerializeField] private SlimeRunawayState runawayState;
    [SerializeField] private DecisionState isInRangeDecisionState;
    [SerializeField] private DecisionState isTooCloseDecisionState;
    [SerializeField] private CooldownState followCooldownState;
    [SerializeField] private CooldownState runawayCooldownState;
    [SerializeField] private CooldownState attackCooldownState;
    
    [SerializeField] private SlimeAnimator animator;
    
    protected override void BindStates()
    {
        //throw new System.NotImplementedException();
    }

    protected override void BindAnimator()
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
        
        rangedAttackState.OnStateEnter.AddListener(animator.PlayRangedAttackAnimation);
        rangedAttackState.OnStateExit.AddListener(animator.PlayIdleAnimation);
    }

    protected override void BindSoundPlayer()
    {
        //throw new System.NotImplementedException();
    }

    public override void ResetAllAnimations()
    {
    }
}