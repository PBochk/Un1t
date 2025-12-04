using UnityEngine;

[RequireComponent(typeof(IdleState))]
[RequireComponent(typeof(GlitchFollowState))]
[RequireComponent(typeof(GlitchChargeState))]
[RequireComponent(typeof(GlitchDashState))]
[RequireComponent(typeof(DecisionState))]
[RequireComponent(typeof(CooldownState))]
[RequireComponent(typeof(GlitchView))]
[RequireComponent(typeof(EnemyModelMB))]
[RequireComponent(typeof(DeadState))]
public class GlitchController : EnemyController
{
    private IdleState idleState;
    private GlitchFollowState followState;
    private GlitchDashState dashState;
    private GlitchChargeState chargeState;
    
    protected override void BindStates()
    {
        idleState = GetComponent<IdleState>();
        followState = GetComponent<GlitchFollowState>();
        dashState = GetComponent<GlitchDashState>();
        chargeState = GetComponent<GlitchChargeState>();
    }

    protected override void BindView()
    {
    }

    protected override void MakeTransitions()
    {
        var idleTransition = new UnconditionalTransition(this, followState);
        idleState.MakeTransition(idleTransition);
        var followTransition = new UnconditionalTransition(this, chargeState);
        followState.MakeTransition(followTransition);
        var chargeTransition = new UnconditionalTransition(this, dashState);
        chargeState.MakeTransition(chargeTransition);
        var dashTransition = new UnconditionalTransition(this, idleState);
        dashState.MakeTransition(dashTransition);
    }

    protected override void TurnOffAllHitboxes()
    {
        
    }
}