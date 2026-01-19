using UnityEngine;
using UnityEngine.Events;

public class SlimeFollowState : EnemyState
{
    [SerializeField] public float baseMoveTime { get; protected set; } = 1f;
    public override float MotionTime => baseMoveTime / model.NativeModel.SpeedCoeff;

    private Vector2 startPosition;


    private bool isJumping = false;
    
    protected Rigidbody2D EnemyRb;
    protected float MoveTimer = 0f;
    protected float CurrentMoveTime;
    protected float Distance;
    public Vector2 Direction { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        EnemyRb = GetComponent<Rigidbody2D>();
        PostAwake();
    }

    public override void EnterState(EnemyTargetComponent target)
    {
        Direction = (target.Position - EnemyRb.position).normalized * Distance;
        base.EnterState(target);

        Distance = Mathf.Min((target.Position - EnemyRb.position).magnitude, model.Config.BaseMoveSpeed);

        startPosition = EnemyRb.position;

        CurrentMoveTime = MotionTime;

        MoveTimer = 0f;
        isJumping = true;
        
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