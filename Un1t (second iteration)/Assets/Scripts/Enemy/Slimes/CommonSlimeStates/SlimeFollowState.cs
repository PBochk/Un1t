using UnityEngine;
using UnityEngine.Events;

public class SlimeFollowState : EnemyState
{
    public UnityEvent jumpStart;

    [SerializeField] public float baseMoveTime { get; protected set; } = 1f;
    public override float MotionTime => baseMoveTime / model.NativeModel.SpeedCoeff;

    private Vector2 startPosition;


    private bool isJumping = false;
    
    protected Rigidbody2D EnemyRb;
    protected float MoveTimer = 0f;
    protected float CurrentMoveTime;
    protected float Distance;
    protected Vector2 Direction;

    protected override void Awake()
    {
        base.Awake();
        EnemyRb = GetComponent<Rigidbody2D>();
        PostAwake();
    }

    public override void EnterState(EnemyTargetComponent target)
    {
        base.EnterState(target);

        Distance = Mathf.Min((target.Position - EnemyRb.position).magnitude, model.Config.BaseMoveSpeed);

        startPosition = EnemyRb.position;
        Direction = (target.Position - EnemyRb.position).normalized * Distance;

        CurrentMoveTime = MotionTime;

        MoveTimer = 0f;
        isJumping = true;

        jumpStart?.Invoke();
        
        PostEnterState();
    }

    private void Update()
    {
        if (!isJumping) return;

        MoveTimer += Time.deltaTime;

        var t = Mathf.Clamp01(MoveTimer / CurrentMoveTime);

        var newPosition = startPosition + Direction * t;
        EnemyRb.MovePosition(newPosition);
        
        PostUpdate();

        if (!(MoveTimer >= CurrentMoveTime)) return;
        isJumping = false;
        MoveTimer = 0f;
        ExitState();
    }
    
    protected virtual void PostUpdate() {}
    protected virtual void PostAwake() {}
    protected virtual void PostEnterState() {}
}