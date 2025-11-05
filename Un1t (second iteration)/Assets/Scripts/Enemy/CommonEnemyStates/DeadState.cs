using System.Collections;
using UnityEngine;

//Todo: rethink if DeadState should even persist
public class DeadState : EnemyState
{
    [SerializeField] private float ExitTime = 1.5f;
    public override float MotionTime => ExitTime;

    public override void EnterState(EnemyTargetComponent target)
    {
        base.EnterState(target);
        StartCoroutine(WaitForExit());
    }

    private IEnumerator WaitForExit()
    {
        yield return new WaitForSeconds(ExitTime);
        ExitState();
    }
}