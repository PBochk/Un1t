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
            animator.PlayJumpAnimation();
            animator.AdjustJumpAnimationSpeed(followState.MotionTime);
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
        
        rangedAttackState.OnStateEnter.AddListener(() =>
        {
            animator.AdjustRangedAttackSpeed(rangedAttackState.MotionTime);
            animator.PlayRangedAttackAnimation();
        });
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