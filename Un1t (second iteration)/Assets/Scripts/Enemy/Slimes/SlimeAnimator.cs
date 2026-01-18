using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SlimeAnimator : MonoBehaviour
{
    private const float MeleeAttackBaseTime = 0.88f;
    private const float RangedAttackBaseTime = 0.5f;
    private const float JumpBaseTime = 1f;
    
    [SerializeField] private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAppearAnimation()
    {
        animator.SetTrigger("SlimeAppearAnimation");
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("SlimeDeathAnimation");
    }

    public void PlayJumpAnimation()
    {
        animator.SetTrigger("SlimeJumpAnimation");
    }
    
    public void PlayIdleAnimation()
    {
        animator.SetTrigger("SlimeIdleAnimation");
    }

    public void PlayRangedAttackAnimation()
    {
        animator.SetTrigger("SlimeRangedAnimation");
    }

    public void PlayMeleeAttackAnimation()
    {
        
        animator.SetTrigger("SlimeMeleeAttackAnimation");
    }

    public void AdjustMeleeAttackSpeed(float motionTime)
    {
        var newSpeed = MeleeAttackBaseTime / motionTime;
        animator.SetFloat("MeleeAttackSpeedMultiplier", newSpeed);
    }

    public void AdjustRangedAttackSpeed(float motionTime)
    {
        var newSpeed = RangedAttackBaseTime / motionTime;
        animator.SetFloat("RangedAttackSpeedMultiplier", newSpeed);
    }
    
    public void AdjustJumpAnimationSpeed(float motionTime)
    {
        var newSpeed = JumpBaseTime / motionTime;
        animator.SetFloat("JumpSpeedMultiplier", newSpeed);
    }

    public void ResetAllTriggers()
    {
        animator.ResetTrigger("SlimeAppearAnimation");
        animator.ResetTrigger("SlimeDeathAnimation");
        animator.ResetTrigger("SlimeJumpAnimation");
        animator.ResetTrigger("SlimeIdleAnimation");
        animator.ResetTrigger("SlimeRangedAnimation");
        animator.ResetTrigger("SlimeMeleeAttackAnimation");
    }
    
    
    public void SetPlaybackSpeed(float speed)
    {
        animator.speed = speed;
    }

    public void ResetPlaybackSpeed()
    {
        animator.speed = 1;
    }
}