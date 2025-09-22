using UnityEngine;

//TODO: Consider reanming (EnemyStrategy, perhaps?)
/// <summary>
/// An abstract class for defining enemy behaviour, an atomic thing that determines, what enemy should do right now
/// </summary>
/// <remarks>
/// Used by <see cref="EnemyController"/> and its derivatives
/// </remarks>
public abstract class EnemyState
{
    /// <summary>
    /// Takes enemy current state, determined by data stored in model and target, and making a decision
    /// by calling view and/or rigid body methods
    /// </summary>
    public abstract void MakeDecision(IEnemyTarget target, EnemyView view, EnemyModel model, Rigidbody2D enemyRb);
}