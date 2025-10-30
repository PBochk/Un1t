using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SlimeAnimator : MonoBehaviour
{
    public Animator animator { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
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

    public void SetPlaybackSpeed(float speed)
    {
        animator.speed = speed;
    }

    public void ResetPlaybackSpeed()
    {
        animator.speed = 1;
    }
}