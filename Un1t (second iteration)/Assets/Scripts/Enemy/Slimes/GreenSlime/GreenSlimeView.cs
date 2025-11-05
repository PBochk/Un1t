using UnityEngine;
using UnityEngine.Animations;

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
    [SerializeField] private DeadState deadState;
    
    [SerializeField] private SlimeAnimator animator;
    
    protected override void BindStates()
    {
        //throw new System.NotImplementedException();
    }

    protected override void BindAnimator()
    {
        animator.PlayIdleAnimation();
        
        followState.OnStateEnter.AddListener(() =>
        {
            animator.AdjustJumpAnimationSpeed(followState.MotionTime);
            animator.PlayJumpAnimation();
        });
        
        followCooldownState.OnStateEnter.AddListener(animator.PlayIdleAnimation);
        
        runawayState.OnStateEnter.AddListener(() =>
        {
            animator.AdjustJumpAnimationSpeed(runawayState.MotionTime);
            animator.PlayJumpAnimation();
        });
        
        runawayCooldownState.OnStateEnter.AddListener(animator.PlayIdleAnimation);
        
        rangedAttackState.OnStateEnter.AddListener(() =>
        {
            animator.AdjustRangedAttackSpeed(rangedAttackState.MotionTime);
            animator.PlayRangedAttackAnimation();
        });
        
        attackCooldownState.OnStateEnter.AddListener(animator.PlayIdleAnimation);
        
        rangedAttackState.OnStateExit.AddListener(animator.PlayIdleAnimation);
        deadState.OnStateEnter.AddListener(animator.PlayDeathAnimation);
    }

    protected override void BindSoundPlayer()
    {
        //throw new System.NotImplementedException();
    }

    public override void ResetAllAnimations()
    {
    }
}