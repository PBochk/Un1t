using UnityEngine;

//Player need to inherit this interface, and then it should be passed to enemySpawner
/// <summary>
/// The thing that enemy wants to attack, usually player
/// </summary>
public interface IEnemyTarget
{
    public Vector2 Position { get; }
}