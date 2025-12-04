using UnityEngine;

[RequireComponent(typeof(IdleState))]
[RequireComponent(typeof(GlitchFollowState))]
[RequireComponent(typeof(GlitchChargeState))]
[RequireComponent(typeof(DecisionState))]
[RequireComponent(typeof(CooldownState))]
[RequireComponent(typeof(GlitchView))]
[RequireComponent(typeof(EnemyModelMB))]
[RequireComponent(typeof(DeadState))]
[RequireComponent(typeof(BoxCollider))]
public class GlitchController : EnemyController
{

    private IdleState idleState;
    private GlitchFollowState followState;
    
    protected override void BindStates()
    {
        idleState = GetComponent<IdleState>();
        followState = GetComponent<GlitchFollowState>();
    }

    protected override void BindView()
    {
    }

    protected override void MakeTransitions()
    {
        var idleTransition = new UnconditionalTransition(this, followState);
        idleState.MakeTransition(idleTransition);
    }

    protected override void TurnOffAllHitboxes()
    {
        
    }
}