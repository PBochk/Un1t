using UnityEngine;

public class MeeeleeChaseState : EnemyState
{
    public override void MakeDecision(IEnemyTarget target, EnemyView view, EnemyModel model, Rigidbody2D enemyRb)
    {
        var direction = (target.Position - (Vector2)view.transform.position).normalized;
        enemyRb.linearVelocity = direction * model.Speed;
    }
}