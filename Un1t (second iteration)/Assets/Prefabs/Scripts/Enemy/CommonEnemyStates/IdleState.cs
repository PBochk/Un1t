using System.Collections;
using UnityEngine;

public class IdleState : EnemyState
{
    //Couldn't think of better solution lol
    //Turns out Awake calls immediately once prefab is initializated
    //So we need to wait (one frame in this example)
    //For enemy to get properly initialized by spawner
    //(i should make it a singleton btw)
    public override void EnterState(EnemyTargetComponent target)
    {
        base.EnterState(target);
        StartCoroutine(WaitForNextFrame());
    }

    private IEnumerator WaitForNextFrame()
    {
        yield return new WaitForEndOfFrame();
        ExitState();
    }
}