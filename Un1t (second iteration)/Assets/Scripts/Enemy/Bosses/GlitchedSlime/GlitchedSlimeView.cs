using System.Xml;
using UnityEngine;

public class GlitchedSlimeView : EnemyView
{
    private DeadState deadState;
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
    }

    protected override void BindSoundPlayer()
    {
    }

    public override void ResetAllAnimations()
    {
    }
}