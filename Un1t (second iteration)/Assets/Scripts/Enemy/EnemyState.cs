using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An abstract class for defining enemy behaviour, an atomic thing that determines, what enemy should do right now
/// </summary>
/// <remarks>
/// Used by <see cref="EnemyController"/> and its derivatives
/// </remarks>
[RequireComponent(typeof(EnemyModelMB))]
public abstract class EnemyState : MonoBehaviour
{
    public UnityEvent OnStateEnter;
    public UnityEvent OnStateExit;

    protected EnemyTargetComponent target;
    protected EnemyModelMB model;
    public abstract float MotionTime { get; }
    
    //TODO: ensure that this is not null
    private EnemyStateTransition transition;

    protected virtual void Awake()
    {
        model = GetComponent<EnemyModelMB>();
    }

    public void MakeTransition(EnemyStateTransition transition)
    {
        this.transition = transition;
    }
        
    public virtual void EnterState(EnemyTargetComponent target)
    {
        //Debug.Log($"Entered state: {this}");
        this.target = target;
        //this.model = model;
        OnStateEnter.Invoke();
    }
    
    //TODO: Ensure that a state can only be exited once
    public void ExitState()
    {
        //Debug.Log($"Exited state: {this}");
        //Debug.Log($"Transition: {transition}");
        transition?.PerformTransition();
        OnStateExit.Invoke();
    }
}