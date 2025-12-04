using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyModelMB))]
public abstract class EnemyState : MonoBehaviour
{
    public UnityEvent OnStateEnter = new UnityEvent();
    public UnityEvent OnStateExit = new UnityEvent();

    protected EnemyTargetComponent target;
    protected EnemyModelMB model;

    private EnemyStateTransition transition;

    private bool isActive = false;
    private bool isExited = false;

    public abstract float MotionTime { get; }

    protected virtual void Awake()
    {
        model = GetComponent<EnemyModelMB>();
        if (model == null)
        {
            Debug.LogError($"EnemyState on {gameObject.name} needs EnemyModelMB");
        }
    }

    /// <summary>
    /// Called by the state machine right after creating the state
    /// </summary>
    public void MakeTransition(EnemyStateTransition transition)
    {
        this.transition = transition;
    }

    /// <summary>
    /// Called by the state machine when this state is activated.
    /// </summary>
    public virtual void EnterState(EnemyTargetComponent target)
    {
        if (isActive)
        {
            Debug.LogWarning($"{this} — attempted to enter state twice!");
            return;
        }

        isActive = true;
        isExited = false;

        this.target = target;

        OnStateEnter?.Invoke();
    }

    /// <summary>
    /// Requests the state machine to proceed to the next state
    /// </summary>
    public void ExitState()
    {
        if (!isActive)
        {
            Debug.LogWarning($"{this} — ExitState called while state is not active!");
            return;
        }

        if (isExited) return;
        isExited = true;
        isActive = false;

        OnStateExit?.Invoke();

        if (transition == null)
        {
            Debug.LogWarning($"{this} has no transition set.");
            return;
        }

        transition.PerformTransition();

        // Cleanup
        target = null;
        transition = null;
    }
}
