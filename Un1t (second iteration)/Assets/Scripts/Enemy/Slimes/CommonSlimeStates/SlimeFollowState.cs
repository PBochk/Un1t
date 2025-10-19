using System;
using System.Collections;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class SlimeFollowState : EnemyState
{
    public UnityEvent jumpStart;
    
    //TODO: Make this configurable
    private const float BASE_SPEED = 3f;
    private const float BASE_MOVE_TIME = 1f;
    private const float BASE_RANGE = 0.75f;
    
    private WaitForFixedUpdate physicsUpdate = new WaitForFixedUpdate();
    private float moveTimer = 0;
    private Vector2 startPosition;
    private Vector2 direction;
    
    private Rigidbody2D enemyRb;

    private void Awake()
    {
        enemyRb =  GetComponent<Rigidbody2D>();
    }
    
    public override void EnterState(IEnemyTarget target)
    {
        base.EnterState(target);
        var distance = Mathf.Min(Vector2.Distance(target.Position, enemyRb.position) - BASE_RANGE, BASE_SPEED);
        startPosition = enemyRb.position;
        direction = (target.Position - enemyRb.position).normalized * distance;
        StartCoroutine(Jump());
    }

    //TODO: Maybe make a windup animation
    private IEnumerator Jump()
    {
        Debug.Log("Jump start");
        while (moveTimer <= BASE_MOVE_TIME)
        {
            enemyRb.MovePosition(startPosition + direction * moveTimer /  BASE_MOVE_TIME);
            moveTimer += Time.fixedDeltaTime;
            yield return physicsUpdate;
        }
        moveTimer = 0f;
        ExitState();
    }
}