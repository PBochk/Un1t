using UnityEngine;
using UnityEngine.Events;

public class TelegraphedJumpState  : SlimeFollowState
{
    [SerializeField] private JumpTelegraphVisual telegraphPrefab;
    private JumpTelegraphVisual telegraphInstance;

    protected override void PostAwake()
    {
        telegraphInstance = Instantiate(telegraphPrefab);
        telegraphInstance.Hide();
    }

    protected override void PostEnterState()
    {
        telegraphInstance.Reset();
        telegraphInstance.Show();
        var targetPos = Direction + EnemyRb.position;
        telegraphInstance.transform.position = targetPos;
    }
    
    protected override void PostUpdate()
    {
        var t = Mathf.Clamp01(MoveTimer / CurrentMoveTime);
        telegraphInstance.UpdateFade(1 - t);

        if (t >= 1f)
        {
            telegraphInstance.Hide();
        }
    }
}