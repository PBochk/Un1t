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
    
    protected IEnemyTarget target;
    protected EnemyView view;
    protected EnemyState currentState;
    protected Rigidbody2D rb;
    protected EnemyModel model;
    protected IdleState idleState;

    public UnityEvent onDeath;
    public UnityEvent onHit;

    /// <summary>
    /// Feel free to override it, but don't forget to call base 
    /// </summary>
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        idleState = GetComponent<IdleState>();
        model = new EnemyModel(config.MaxHealth,  config.MaxHealth, config.SpeedScale, config.Damage);
        ChangeState(idleState);
    }

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
        this.target = target;
    }

    protected void ChangeState(EnemyState newState)
    {
        //TODO: Make it impossible to change state when current is not exited or interrupted yet
        Debug.Log($"Changed state: {currentState} -> {newState}");
        currentState = newState;
        currentState.EnterState(target, model);
    }

    //To be reconsidered, what to pass here
    public virtual void GetHit(int damage)
    {
        model = model.WithHealth(Mathf.Clamp(model.Health - damage, 0, config.MaxHealth));
        if (model.Health <= 0)
        {
            onDeath.Invoke();
        }
    }

    public virtual void ApplyEffect()
    {
    }
    
}