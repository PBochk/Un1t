using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdleState : EnemyState
{
    [SerializeField] private float EnterTime = 0.4f;
    private const float DISPERSION = 0.3f;
    public override float MotionTime => EnterTime;
    public override void EnterState(EnemyTargetComponent target)
    {
        base.EnterState(target);
        StartCoroutine(WaitForEnter());
    }

    private IEnumerator WaitForEnter()
    {
        yield return new WaitForSeconds(EnterTime);
        yield return new WaitForSeconds(Random.value * DISPERSION);
        ExitState();
    }
}