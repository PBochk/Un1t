using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class GlitchDashState : EnemyState
{
    [Header("Dash Settings")]
    public float distance = 7.5f;
    public float duration = 0.4f;

    public override float MotionTime => duration;

    public AnimationCurve dashCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Rigidbody2D rb;
    private WaitForFixedUpdate physicsUpdate = new WaitForFixedUpdate();
    private bool isDashing = false;
    private float timer = 0;
    private Vector2 startPos;
    private Vector2 direction;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void EnterState(EnemyTargetComponent target)
    {
        base.EnterState(target);

        if (target == null) return;

        startPos = rb.position;
        direction = (target.Position - rb.position).normalized;
        timer = 0;
        isDashing = true;
    }

    private void FixedUpdate()
    {
        if (!isDashing || duration < timer) return;
        var t = timer / duration;
        var curveT = dashCurve.Evaluate(t);
        rb.MovePosition(startPos + curveT * distance * direction);
        timer += Time.fixedDeltaTime;
        if (timer > duration)
        {
            isDashing = false;
            ExitState();
        }
    }
}