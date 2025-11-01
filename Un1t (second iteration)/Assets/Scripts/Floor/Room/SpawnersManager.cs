using System.Collections.Generic;
using UnityEngine;

public class SpawnersManager
{
    public IReadOnlyList<RoomEnemySpawner> EnemySpawners => enemySpawners;

    private readonly List<RoomEnemySpawner> enemySpawners;

    public SpawnersManager()
    {
        enemySpawners = new List<RoomEnemySpawner>();
    }

    //TODO: implement fully
    //This version is for demonstration purpose only.

    public void SetSpawners(EnemyController enemy, Vector2 position, EnemyTargetComponent enemyTarget, RoomEnemySpawner spawner)
    {
        enemySpawners.Add(RoomEnemySpawner.Instantiate(spawner));
        enemySpawners[0].SetCreationEnemy(enemy, position, enemyTarget);
        enemySpawners[0].CreateEnemy();
    }

}
