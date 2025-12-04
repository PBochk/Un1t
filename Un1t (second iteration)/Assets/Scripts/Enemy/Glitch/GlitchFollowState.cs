using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GlitchFollowState : EnemyState
{
    public override float MotionTime { get; }

    //TODO: Use config
    private const float TestRange = 3f;
    private const float TestSpeed = 5f;
    private const float TestRangeSqr = TestRange * TestRange;
    
    private bool isFollowActive;
    private Rigidbody2D rb;
    
    public override void EnterState(EnemyTargetComponent target)
    {
        base.EnterState(target);
        isFollowActive = true;
    }

    protected override void Awake()
    {
        base.Awake();
        OnStateExit.AddListener(() => { isFollowActive = false; });
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        if (!isFollowActive) return;
        var delta = (Vector2)target.transform.position - rb.position;
        if (delta.sqrMagnitude < TestRangeSqr)
        {
            ExitState();
            return;
        }
        var direction = delta.normalized;
        rb.MovePosition(TestSpeed * Time.fixedDeltaTime * direction + rb.position);
    }
}