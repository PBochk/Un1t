public abstract class EnemyStateTransition
{
    protected readonly EnemyController controller;

    protected EnemyStateTransition(EnemyController controller)
    {
        this.controller = controller;
    }

    public abstract void PerformTransition();
}