using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An abstract class for defining enemy behaviour, an atomic thing that determines, what enemy should do right now
/// </summary>
/// <remarks>
/// Used by <see cref="EnemyController"/> and its derivatives
/// </remarks>
public abstract class EnemyState : MonoBehaviour
{
    public UnityEvent OnStateEnter;
    public UnityEvent OnStateExit;

    protected IEnemyTarget target;
    protected EnemyModel model;
    
    //TODO: ensure that this is not null
    private EnemyStateTransition transition;

    public void MakeTransition(EnemyStateTransition transition)
    {
        this.transition = transition;
    }
        
    public virtual void EnterState(IEnemyTarget target, EnemyModel model)
    {
        Debug.Log($"Entered state: {this}");
        this.target = target;
        this.model = model;
    }
    
    //TODO: Ensure that a state can only be exited once
    protected void ExitState()
    {
        Debug.Log($"Exited state: {this}");
        Debug.Log($"Transition: {transition}");
        transition.PerformTransition();
        OnStateExit.Invoke();
    }
}