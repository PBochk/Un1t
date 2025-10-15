using UnityEngine;

//Player need to inherit this interface, and then it should be passed to enemySpawner at Initialization step of entry point
/// <summary>
/// The thing that enemy wants to attack, usually player, used by <see cref="EnemyController"/>
/// <see cref="EnemySpawner"/> and <see cref="EnemyState"/> and its derivatives
/// </summary>
//TODO: Potentially replace this with TargetComponent or something like that 
public interface IEnemyTarget
{
    public Vector2 Position { get; }
}