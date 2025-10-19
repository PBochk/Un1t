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
[RequireComponent(typeof(EnemyModelMB))]
public abstract class EnemyController : MonoBehaviour
{
    public IEnemyTarget Target { get; private set; }
   
    //protected EnemyView View;
    protected Rigidbody2D Rb;
    protected IdleState IdleState;
    protected EnemyModelMB ModelMB;
    protected EnemyState CurrentState;
    
    //public EnemyModelMB Model => ModelMB;
    
    public UnityEvent onDeath;
    public UnityEvent onHit;

    protected void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        IdleState = GetComponent<IdleState>();
        BindModel();
        BindView();
        BindStates();
        MakeTransitions();
        ChangeState(IdleState);
    }

    protected virtual void BindModel()
    {
        ModelMB = GetComponent<EnemyModelMB>();
    }
    
    //TODO: Consider removing
    protected abstract void BindStates();

    protected abstract void BindView();

    protected abstract void MakeTransitions();
    
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
        CurrentState.EnterState(Target);
    }
}