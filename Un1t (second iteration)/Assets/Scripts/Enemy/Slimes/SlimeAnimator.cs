using UnityEngine;

public class SlimeAnimator : MonoBehaviour
{
    public Animator animator { get; private set; }
    public void PlayJumpAnimation()
    {
        animator.SetTrigger("SlimeJumpAnimation");
    }
    
    public void PlayIdleAnimation()
    {
        animator.SetTrigger("SlimeIdleAnimation");
    }
}