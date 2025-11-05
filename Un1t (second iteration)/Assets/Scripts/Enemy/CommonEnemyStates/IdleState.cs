using System.Collections;
using UnityEngine;

public class IdleState : EnemyState
{
    [SerializeField] private float EnterTime = 0.4f;
    public override float MotionTime => EnterTime;
    public override void EnterState(EnemyTargetComponent target)
    {
        base.EnterState(target);
        StartCoroutine(WaitForEnter());
    }

    private IEnumerator WaitForEnter()
    {
        yield return new WaitForSeconds(EnterTime);
        ExitState();
    }
}