using UnityEngine;

public class MeleeChaseState : EnemyState
{
    public override void MakeDecision(IEnemyTarget target, EnemyModel model)
    {
        //var direction = (target.Position - (Vector2)view.transform.position).normalized;
        //enemyRb.linearVelocity = direction * model.Speed;
    }
}