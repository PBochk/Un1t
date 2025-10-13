using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An abstract class, it's derivatives intended to determine enemy's behaviour,
/// be responsible for changing between states / strategies and sending/receiving Events
/// </summary>
/// <remarks>
/// Guideline for making a new enemy:
/// -> Create a folder for its scripts (duh)
/// -> Write an EnemyView class
/// -> Write several EnemyState classes
/// -> Make a new controller for it, create several fields, one for each EnemyState that the new enemy has
/// -> (???) next is the possible create configuration for stats, but i'll figure this out later
/// -> make a prefab variant of base enemy and attach controller and view to it
/// </remarks>
[RequireComponent(typeof(IdleState))]
public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyConfig config;
    
    //Those are used by state transitions
    public IEnemyTarget Target { get; private set; }
    public EnemyModel Model { get; private set; }
   
    //protected EnemyView View;
    protected Rigidbody2D Rb;
    protected IdleState IdleState;

    protected EnemyState CurrentState;
    
    public UnityEvent onDeath;
    public UnityEvent onHit;

    //TODO: Make this awake unoverridable, make several abstract methods that describe initialization process
    //TODO: Every controller awake should end with changing to idle state
    /// <summary>
    /// Feel free to override it, but don't forget to call base 
    /// </summary>
    protected void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        IdleState = GetComponent<IdleState>();
        BindModel();
        BindView();
        BindStates();
        MakeTransitions();
        Model = new EnemyModel(config.MaxHealth,  config.MaxHealth, config.SpeedScale, config.Damage);
        ChangeState(IdleState);
    }

    protected abstract void BindModel();
    
    protected abstract void BindStates();

    protected abstract void BindView();

    protected abstract void MakeTransitions();
    

    /// <summary>
    /// Feel free to override it, but don't forget to call base 
    /// </summary>
    protected virtual void FixedUpdate()
    {
        MakeDecision();
    }

    /// <summary>
    /// Feel free to override it, but don't forget to call base 
    /// </summary>
    protected virtual void MakeDecision()
    {
        //currentState.MakeDecision(target, model);
    }

    /// <summary>
    /// Normally used on the initialization step, but can be changed in the lifetime for example, to make enemy aggro
    /// to the fake player
    /// </summary>
    public void SetTarget(IEnemyTarget target)
    {
        if (target is null)
            return;
        this.Target = target;
    }

    //It needed to be called from EnemyStateTransition
    //That's should not be a problem since usually only the 
    public void ChangeState(EnemyState newState)
    {
        //TODO: Make it impossible to change state when current is not exited or interrupted yet
        Debug.Log($"Changed state: {CurrentState} -> {newState}");
        CurrentState = newState;
        CurrentState.EnterState(Target, Model);
    }
}