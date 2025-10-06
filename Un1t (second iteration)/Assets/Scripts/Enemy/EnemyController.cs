using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyConfig config;
    
    protected IEnemyTarget target;
    protected EnemyView view;
    protected EnemyState currentState;
    protected Rigidbody2D rb;
    protected EnemyModel model;

    public UnityEvent onDeath;
    public UnityEvent onHit;

    protected virtual void Awake()
    {
        view = GetComponent<EnemyView>();
        rb = GetComponent<Rigidbody2D>();
        model = new EnemyModel(config.MaxHealth,  config.MaxHealth, config.SpeedScale, config.Damage);
    }

    protected virtual void FixedUpdate()
    {
        MakeDecision();
    }

    protected virtual void MakeDecision()
    {
        currentState.MakeDecision(target, model);
    }

    public void SetTarget(IEnemyTarget target)
    {
        this.target = target;
    }

    protected void ChangeState(EnemyState newState)
    {
        currentState.enabled = false;
        currentState = newState;
        currentState.enabled = true;
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