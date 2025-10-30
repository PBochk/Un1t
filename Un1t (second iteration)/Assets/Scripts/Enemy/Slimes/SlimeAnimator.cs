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

    public void PlayMeleeAttackAnimation()
    {
        
        animator.SetTrigger("SlimeMeleeAttackAnimation");
    }
}