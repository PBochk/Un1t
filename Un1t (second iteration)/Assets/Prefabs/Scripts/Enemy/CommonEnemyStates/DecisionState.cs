public class DecisionState : EnemyState
{
    public override void EnterState(EnemyTargetComponent target)
    {
        ExitState();
    }
}