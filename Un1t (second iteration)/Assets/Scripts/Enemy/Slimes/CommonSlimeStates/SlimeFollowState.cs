using System;
using System.Collections;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D),
    typeof(SlimeAnimator))]
public class SlimeFollowState : EnemyState
{
    public UnityEvent jumpStart;
    
    //TODO: Make this configurable
    private const float BASE_SPEED = 3f;
    private const float BASE_MOVE_TIME = 1f;
    private const float BASE_RANGE = 0.75f;
    
    //TODO: Make this configurable
    //private WaitForSeconds jumpDelay = new WaitForSeconds(1f);
    private WaitForFixedUpdate physicsUpdate = new WaitForFixedUpdate();
    private float moveTimer = 0;
    private Vector2 startPosition;
    private Vector2 direction;
    
    private Rigidbody2D enemyRb;
    
    //TODO: Logic state should not be dependant on animator class, make an event that is view subscribed on
    private SlimeAnimator animator;

    private void Awake()
    {
        enemyRb =  GetComponent<Rigidbody2D>();
        animator = GetComponent<SlimeAnimator>();
    }
    
    public override void EnterState(IEnemyTarget target, EnemyModel model)
    {
        base.EnterState(target, model);
        var distance = Mathf.Min(Vector2.Distance(target.Position, enemyRb.position) - BASE_RANGE, BASE_SPEED);
        startPosition = enemyRb.position;
        direction = (target.Position - enemyRb.position).normalized * distance;
        StartCoroutine(Jump());
    }

    //TODO: Maybe make a windup animation
    private IEnumerator Jump()
    {
        animator.PlayJumpAnimation();
        Debug.Log("Jump start");
        //jumpStart.Invoke();
        while (moveTimer <= BASE_MOVE_TIME)
        {
            enemyRb.MovePosition(startPosition + direction * moveTimer /  BASE_MOVE_TIME);
            moveTimer += Time.fixedDeltaTime;
            yield return physicsUpdate;
        }
        moveTimer = 0f;
        //StartCoroutine(AfterJumpDelay());
        animator.PlayIdleAnimation();
        ExitState();
    }

    //TODO: Change to movement delay state instead
    // private IEnumerator AfterJumpDelay()
    // {
    //     Debug.Log("Delay after jump");
    //     animator.PlayIdleAnimation();
    //     ExitState();
    // }
}