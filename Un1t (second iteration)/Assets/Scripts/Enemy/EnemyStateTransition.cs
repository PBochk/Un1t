public class EnemyStateTransition
{
    public delegate bool Condition(IEnemyTarget target, EnemyModel model);
    
    private readonly EnemyController controller;
    private Condition checkCondition;
    
    public EnemyStateTransition(EnemyController controller, Condition checkCondition)
    {
        this.controller = controller;
        this.checkCondition = checkCondition;
    }
}