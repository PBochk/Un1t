using UnityEngine;
using UnityEngine.Events;

public class SlimeFollowState : EnemyState
{
    public UnityEvent jumpStart;

    [SerializeField] public float baseMoveTime { get; private set; } = 1f;
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

        var distance = model.Config.BaseMoveSpeed;

        startPosition = enemyRb.position;
        direction = (target.Position - enemyRb.position).normalized * distance;

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