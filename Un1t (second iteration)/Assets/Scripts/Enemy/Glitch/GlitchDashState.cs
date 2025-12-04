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

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void EnterState(EnemyTargetComponent target)
    {
        base.EnterState(target);

        if (target == null) return;

        Vector2 startPos = rb.position;
        Vector2 direction = (target.Position - rb.position).normalized;

        // Запуск корутины через безопасный RunStateCoroutine
        RunStateCoroutine(DashRoutine(startPos, direction));
    }

    private IEnumerator DashRoutine(Vector2 startPos, Vector2 direction)
    {
        float timer = 0f;

        while (timer < duration)
        {
            float t = timer / duration;

            // Применяем кривую
            float curveT = dashCurve.Evaluate(t);

            rb.MovePosition(startPos + curveT * distance * direction);

            timer += Time.fixedDeltaTime;
            yield return physicsUpdate;
        }

        // Финальная позиция (на случай неточного t)
        rb.MovePosition(startPos + direction * distance);

        // Завершаем состояние
        ExitState();
    }
}