using UnityEngine;

public class ConditionalTransition : EnemyStateTransition
{
    public delegate bool Condition(IEnemyTarget target);
    
    private readonly Condition condition;
    private readonly EnemyState trueState;
    private readonly EnemyState falseState;

    public ConditionalTransition(EnemyController controller, Condition condition, EnemyState trueState, EnemyState falseState) : base(controller)
    {
        this.condition = condition;
        this.trueState = trueState;
        this.falseState = falseState;
    }

    public override void PerformTransition()
    {
        controller.ChangeState(condition(controller.Target) ? trueState : falseState);
    }
}