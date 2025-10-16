using UnityEngine;

public class BlueSlimeView : EnemyView
{
    [SerializeField] private SlimeFollowState followState;
    [SerializeField] private SlimeMeleeAttackState meleeState;
    [SerializeField] private CooldownState afterJumpCooldown;
    [SerializeField] private CooldownState afterAttackCooldown;
    [SerializeField] private DecisionState decisionState;
    [SerializeField] private SlimeAnimator animator;

    protected override void BindModel()
    {
        throw new System.NotImplementedException();
    }

    protected override void BindStates()
    {
        throw new System.NotImplementedException();
    }

    protected override void BindAnimator()
    {
        throw new System.NotImplementedException();
    }

    protected override void BindSoundPlayer()
    {
        throw new System.NotImplementedException();
    }
}