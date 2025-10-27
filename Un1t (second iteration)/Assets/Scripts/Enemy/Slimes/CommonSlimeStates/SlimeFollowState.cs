using System;
using System.Collections;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class SlimeFollowState : EnemyState
{
    public UnityEvent jumpStart;
    
    [SerializeField] public float baseMoveTime { get; private set; } = 1f;
    
    private WaitForFixedUpdate physicsUpdate = new WaitForFixedUpdate();
    private float moveTimer = 0;
    private Vector2 startPosition;
    private Vector2 direction;
    
    private Rigidbody2D enemyRb;

    protected override void Awake()
    {
        base.Awake();
        enemyRb =  GetComponent<Rigidbody2D>();
    }
    
    public override void EnterState(EnemyTargetComponent target)
    {
        base.EnterState(target);
        var distance = Mathf.Min(Vector2.Distance(target.Position, enemyRb.position) - model.Config.AggroRange, model.Config.BaseMoveSpeed);
        startPosition = enemyRb.position;
        direction = (target.Position - enemyRb.position).normalized * distance;
        //TODO: Fix Possible DivisionByZeroException 
        StartCoroutine(Jump(baseMoveTime / model.NativeModel.SpeedCoeff));
    }

    //TODO: Maybe make a windup animation
    private IEnumerator Jump(float moveTime)
    {
        while (moveTimer <= moveTime)
        {
            enemyRb.MovePosition(startPosition + direction * moveTimer /  moveTime);
            moveTimer += Time.fixedDeltaTime;
            yield return physicsUpdate;
        }
        moveTimer = 0f;
        ExitState();
    }
}