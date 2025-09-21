using UnityEngine;

//TODO: Consider reanming (EnemyStrategy, perhaps?)
public abstract class EnemyState
{
    public abstract void MakeDecision(IEnemyTarget target, EnemyView view, EnemyModel model, Rigidbody2D enemyRb);
}