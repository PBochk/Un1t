using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SmartRunawayState : EnemyState
{
    [Header("Runaway Configuration")]
    [SerializeField] private float roomRadius = 20f;
    [SerializeField] private float[] runawayAngles = { -30f, 0f, 30f };
    public float BaseMoveTime { get; private set; } = 0.75f;
    
    public UnityEvent jumpStart;
    public Vector2 RoomCenter { get; set; }
    public override float MotionTime => BaseMoveTime / model.NativeModel.SpeedCoeff;

    private float moveTimer = 0f;
    private Vector2 startPosition;
    private Vector2 direction;

    private Rigidbody2D enemyRb;

    private bool isJumping = false;
    private float currentMoveTime;

    protected override void Awake()
    {
        base.Awake();
        enemyRb =  GetComponent<Rigidbody2D>();
    }

    private Vector2 GetPreferredRunawayPoint(EnemyTargetComponent target)
    {
        var myPos = enemyRb.position;
        var targetPos = target.Position;
        var awayFromTarget = (myPos - targetPos).normalized;
        var moveDist = model.Config.BaseMoveSpeed / 3;
    
        var sqrRoomRadius = roomRadius * roomRadius;
    
        var bestPoint = myPos + awayFromTarget * moveDist;
        var maxSqrDistToTarget = -1f;

        var sqrDistToCenter = (bestPoint - RoomCenter).sqrMagnitude;
        if (sqrDistToCenter <= sqrRoomRadius)
        {
            maxSqrDistToTarget = (bestPoint - targetPos).sqrMagnitude;
        }

        foreach (var angle in runawayAngles)
        {
            Vector2 rotatedDirection = Quaternion.Euler(0, 0, angle) * awayFromTarget;
            var potentialPoint = myPos + rotatedDirection * moveDist;

            if (!((potentialPoint - RoomCenter).sqrMagnitude <= sqrRoomRadius)) continue;
            var sqrDistToTarget = (potentialPoint - targetPos).sqrMagnitude;
            
            if (!(sqrDistToTarget > maxSqrDistToTarget)) continue;
            maxSqrDistToTarget = sqrDistToTarget;
            bestPoint = potentialPoint;
        }

        return bestPoint;
    }
    
    public override void EnterState(EnemyTargetComponent target)
    {
        base.EnterState(target);

        var runawayPoint = GetPreferredRunawayPoint(target);

        startPosition = enemyRb.position;
        direction = (runawayPoint - startPosition).normalized * model.Config.BaseMoveSpeed / 3;

        currentMoveTime = MotionTime;

        moveTimer = 0f;
        isJumping = true;

        jumpStart?.Invoke();
    }
    
    private void Update()
    {
        if (!isJumping) return;

        moveTimer += Time.deltaTime;

        var t = Mathf.Clamp01(moveTimer / currentMoveTime);

        var newPosition = startPosition + direction * t;
        enemyRb.MovePosition(newPosition);

        if (!(moveTimer >= currentMoveTime)) return;
        isJumping = false;
        moveTimer = 0f;
        ExitState();
    }
}