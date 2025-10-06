using System;
using System.Collections;
using System.Dynamic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SlimeFollowState : EnemyState
{
    public event Action JumpStart;
    
    private const float BASE_SPEED = 3f;
    private const float BASE_MOVE_TIME = 1f;
    
    private WaitForSeconds jumpDelay = new WaitForSeconds(1f);
    private WaitForFixedUpdate physicsUpdate = new WaitForFixedUpdate();
    private bool onDelay = false;
    private bool moving = false;
    private float moveTimer = 0;
    private Vector2 startPosition;
    private Vector2 direction;
    
    private Rigidbody2D enemyRb;
    private BlueSlimeView view;

    private void Awake()
    {
        enemyRb =  GetComponent<Rigidbody2D>();
        view = GetComponent<BlueSlimeView>();
    }
    
    public override void MakeDecision(IEnemyTarget target, EnemyModel model)
    {
        if (onDelay || moving) return;
        var distance = Mathf.Min(Vector2.Distance(target.Position, enemyRb.position), BASE_SPEED);
        startPosition = enemyRb.position;
        direction = (target.Position - enemyRb.position).normalized * distance;
        StartCoroutine(Jump());
    }

    private IEnumerator Jump()
    {
        moving = true;
        view.PlayJumpAnimation();
        JumpStart?.Invoke();
        while (moveTimer <= BASE_MOVE_TIME)
        {
            enemyRb.MovePosition(startPosition + direction * moveTimer /  BASE_MOVE_TIME);
            moveTimer += Time.fixedDeltaTime;
            yield return physicsUpdate;
        }
        moving = false;
        moveTimer = 0f;
        StartCoroutine(AfterJumpDelay());
    }

    private IEnumerator AfterJumpDelay()
    {
        view.PlayIdleAnimation();
        onDelay = true;
        yield return jumpDelay;
        onDelay = false;
    }
}