using System.Xml;
using UnityEngine;

[RequireComponent(typeof(SlimeAnimator))]
[RequireComponent(typeof(EnemySoundPlayer))]
public class GlitchedSlimeView : EnemyView
{
    [SerializeField] private TelegraphedJumpState followState;
    [SerializeField] private SmartRunawayState runawayState;
    private DeadState deadState;
    private SlimeAnimator animator;
    private EnemySoundPlayer soundPlayer;
    
    protected override void BindStates()
    {
        deadState = GetComponent<DeadState>();
        deadState.OnStateEnter.AddListener(() =>
        {
            GameOverUI.Instance.OnBossDeath();
        });

    }

    protected override void BindAnimator()
    {
        animator = GetComponent<SlimeAnimator>();
    }

    protected override void BindSoundPlayer()
    {
        soundPlayer = GetComponent<EnemySoundPlayer>();
        followState.OnStateEnter.AddListener(() =>
        {
            soundPlayer.PlayMoveSound();
        });
        runawayState.OnStateEnter.AddListener(() =>
        {
            soundPlayer.PlayMoveSound();
        });
        deadState.OnStateEnter.AddListener(() =>
        {
            soundPlayer.PlaydDeathSound();
        });
    }

    public override void ResetAllAnimations()
    {
    }
}