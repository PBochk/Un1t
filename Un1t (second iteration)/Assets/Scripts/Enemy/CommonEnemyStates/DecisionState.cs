using UnityEngine;

public class DecisionState : EnemyState
{
    [Tooltip("Usually enemies can have multiple different Decision states, so this is made to differentiate them in inspector")]
    [SerializeField] private string label;
    public override void EnterState(EnemyTargetComponent target)
    {
        base.EnterState(target);
        ExitState();
    }
}