using UnityEngine;
using UnityEngine.Events;

//TODO: Consider reanming (EnemyStrategy, perhaps?)
/// <summary>
/// An abstract class for defining enemy behaviour, an atomic thing that determines, what enemy should do right now
/// </summary>
/// <remarks>
/// Used by <see cref="EnemyController"/> and its derivatives
/// </remarks>
public abstract class EnemyState : MonoBehaviour
{
    public UnityEvent OnStateEnter;
    public UnityEvent<bool> OnStateExit;

    protected IEnemyTarget target;
    protected EnemyModel model;

    public virtual void EnterState(IEnemyTarget target, EnemyModel model)
    {
        Debug.Log($"Entered state: {this}");
        this.target = target;
        this.model = model;
    }
    //TODO: Ensure that a state can only be exited once
    protected void ExitState(bool condition)
    {
        Debug.Log($"Exited state: {this}");
        OnStateExit.Invoke(condition);
    }
}