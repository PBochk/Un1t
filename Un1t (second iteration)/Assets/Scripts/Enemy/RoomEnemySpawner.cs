using System;
using UnityEngine;

/// <summary>
/// Is used to spawn enemies in rooms
/// </summary>
public class RoomEnemySpawner : MonoBehaviour
{
    private EnemyController enemyPrefab;
    private Vector2 spawnPosition;
    private EnemyTargetComponent target;

    /// <summary>
    /// This is a method to initialize an enemy spawner, use this after creating instance of RoomEnemySpawner
    /// </summary>
    /// <param name="enemyPrefab"></param>
    /// <param name="position"></param>
    /// <param name="target"></param>
    public void SetCreationEnemy(EnemyController enemyPrefab, Vector2 position, EnemyTargetComponent target)
    {
        this.enemyPrefab = enemyPrefab;
        this.spawnPosition = position;
        this.target = target;
    }

    /// <summary>
    /// Spawns enemy according to set configuration
    /// </summary>
    /// <returns>An instance of created anemy</returns>
    public EnemyController CreateEnemy()
    {
        if (enemyPrefab is null || target is null)
            throw new Exception("Room enemy spawner must be initialized through SetCreationEnemy first");
        var enemy = Instantiate(enemyPrefab,  spawnPosition, Quaternion.identity);
        enemy.SetTarget(target);
        return enemy;
    }
}