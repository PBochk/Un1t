using System.Collections;
using UnityEngine;

public class GlitchChargeState : EnemyState
{
    [Header("Charge Settings")]
    [SerializeField]
    private float chargeDelay = 0.5f;

    [Header("Telegraphing")]
    [SerializeField]
    private GameObject telegraphingObject;

    public override float MotionTime => chargeDelay;

    private readonly WaitForFixedUpdate physicsUpdate = new WaitForFixedUpdate();

    public override void EnterState(EnemyTargetComponent target)
    {
        base.EnterState(target);
        
        telegraphingObject.SetActive(true);
        OnStateExit.AddListener(() =>
        {
            telegraphingObject.SetActive(false);
        });

        RunStateCoroutine(ChargeRoutine(target));
    }

    private IEnumerator ChargeRoutine(EnemyTargetComponent target)
    {
        var timer = 0f;

        while (timer < chargeDelay)
        {
            timer += Time.fixedDeltaTime;

            var direction = (target.Position - (Vector2)telegraphingObject.transform.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            telegraphingObject.transform.rotation = Quaternion.Euler(0, 0, angle);

            yield return physicsUpdate;
        }

        ExitState();
    }
}