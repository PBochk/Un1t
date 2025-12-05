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
    public void SetSpawners(EnemyController enemy, Vector2 position, EnemyTargetComponent enemyTarget, RoomEnemySpawner spawner, Transform parent)
    {
        enemySpawners.Add(RoomEnemySpawner.Instantiate(spawner, parent));
        enemySpawners[0].SetCreationEnemy(enemy, position, enemyTarget);
        EnemyController createdEnemy = enemySpawners[0].CreateEnemy();

        //Next functional is for demo only.
        //RoomManager.EnemiesCount++;
        createdEnemy.Model.OnDeath.AddListener(EnemyDead);

        void EnemyDead()
        {
            if (true/*--RoomManager.EnemiesCount == 0*/)
            {
                GameObject.FindWithTag("LevelEnder").GetComponent<EventParent>().NotifyLevelEnded();
            }          

        }

    }
}
