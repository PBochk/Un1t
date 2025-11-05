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
        var newSpeed = motionTime / MeleeAttackBaseTime;
        animator.SetFloat("MeleeAttackSpeedMultiplier", motionTime);
    }

    public void AdjustRangedAttackSpeed(float motionTime)
    {
        var newSpeed = motionTime / RangedAttackBaseTime;
        animator.SetFloat("RangedAttackSpeedMultiplier", motionTime);
    }
    
    public void AdjustJumpAnimationSpeed(float motionTime)
    {
        var newSpeed = motionTime / JumpBaseTime;
        animator.SetFloat("JumpSpeedMultiplier", motionTime);
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