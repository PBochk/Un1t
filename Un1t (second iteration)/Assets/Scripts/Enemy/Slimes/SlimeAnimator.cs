using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SlimeAnimator : MonoBehaviour
{
    public Animator animator { get; private set; }
    
    [Header("Atlases")]
    [SerializeField] private Texture2D AppearAtlas;
    [SerializeField] private Texture2D JumpAtlas;
    [SerializeField] private Texture2D RangedAtlas;
    [SerializeField] private Texture2D MeleeAtlas;
    [SerializeField] private Texture2D DeathAtlas;
    
    [Header("Object references")]
    [SerializeField] private SpriteRenderer sprite;
    

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

    public void SetPlaybackSpeed(float speed)
    {
        animator.speed = speed;
    }

    public void ResetPlaybackSpeed()
    {
        animator.speed = 1;
    }
}