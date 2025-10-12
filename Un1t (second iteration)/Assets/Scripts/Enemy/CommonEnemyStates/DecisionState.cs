public class DecisionState : EnemyState
{
    public override void EnterState(IEnemyTarget target, EnemyModel model)
    {
        ExitState();
    }
}