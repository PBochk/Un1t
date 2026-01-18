using UnityEngine;
using UnityEngine.Events;

public class SlimeRunawayState : EnemyState
{
    public UnityEvent jumpStart;

    [SerializeField] public float baseMoveTime { get; private set; } = 0.75f;
    public override float MotionTime => baseMoveTime / model.NativeModel.SpeedCoeff;

    private float moveTimer = 0f;
    private Vector2 startPosition;
    private Vector2 direction;

    private Rigidbody2D enemyRb;

    private bool isJumping = false;
    private float currentMoveTime;

    protected override void Awake()
    {
        base.Awake();
        enemyRb = GetComponent<Rigidbody2D>();
    }

    public override void EnterState(EnemyTargetComponent target)
    {
        base.EnterState(target);

        float distance = model.Config.BaseMoveSpeed;

        startPosition = enemyRb.position;
        direction = (enemyRb.position - target.Position).normalized * distance;

        currentMoveTime = MotionTime;

        moveTimer = 0f;
        isJumping = true;

        jumpStart?.Invoke();
    }

    private void Update()
    {
        if (!isJumping) return;

        moveTimer += Time.deltaTime;

        float t = Mathf.Clamp01(moveTimer / currentMoveTime);

        Vector2 newPosition = startPosition + direction * t;
        enemyRb.MovePosition(newPosition);

        if (moveTimer >= currentMoveTime)
        {
            isJumping = false;
            moveTimer = 0f;
            ExitState();
        }
    }
}