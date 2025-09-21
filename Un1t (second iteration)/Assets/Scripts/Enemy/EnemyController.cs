using System;
using UnityEngine;
using UnityEngine.Events;

//TODO: Reconsider name
public abstract class EnemyController : MonoBehaviour
{
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
    }

    protected virtual void FixedUpdate()
    {
        MakeDecision();
    }

    protected virtual void MakeDecision()
    {
        currentState.MakeDecision(target, view, model, rb);
    }

    public void SetTarget(IEnemyTarget target)
    {
        this.target = target;
    }
}