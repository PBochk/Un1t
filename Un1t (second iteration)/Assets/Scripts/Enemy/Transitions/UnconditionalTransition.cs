using UnityEngine;

public class UnconditionalTransition : EnemyStateTransition
{
    private readonly EnemyState nextState;

    public UnconditionalTransition(EnemyController controller, EnemyState nextState) : base(controller)
    {
        this.nextState = nextState;
    }

    public override void PerformTransition()
    {
        //Debug.Log($"Performing transition -> {nextState}");
        controller.ChangeState(nextState);
    }
}
