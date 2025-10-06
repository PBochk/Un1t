using System.Collections;
using UnityEngine;

//TODO: Make EntryState or something like that instead, that does exactly this
public class IdleState : EnemyState
{
    //Couldn't think of better solution lol
    //Turns out Awake calls immediately once prefab is initializated
    //So we need to wait (one frame in this example)
    //For enemy to get properly initialized by spawner
    //(i should make it a singleton btw)
    public override void EnterState(IEnemyTarget target, EnemyModel model)
    {
        base.EnterState(target, model);
        StartCoroutine(WaitForNextFrame());
    }

    private IEnumerator WaitForNextFrame()
    {
        yield return new WaitForEndOfFrame();
        ExitState(true);
    }
}