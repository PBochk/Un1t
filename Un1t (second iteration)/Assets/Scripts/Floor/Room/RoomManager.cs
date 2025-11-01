using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using UnityEngine;


/// <summary>
/// Manages everything inside one room
/// Keeps track of all RoomEntities
/// </summary>
public class RoomManager : MonoBehaviour
{
    private RoomEnemySpawner enemySpawner;

    private SpawnersManager spawnersManager;

    private ImmutableList<GameObject> entities;
    private ImmutableList<GameObject> outerWalls;
    private IReadOnlyList<EnemyController> spawnableEnemies;

    private readonly static Range shurfesCountRange = new(2, 5);

    private int shurfesCount;

    public IReadOnlyList<GameObject> Entities => entities;
    public IReadOnlyList<GameObject> OuterWalls => outerWalls;


    /// <summary>
    /// Creates all RoomEntities in this room 
    /// RoomEntities are placed relative to the room's position
    /// Shall only be called one time for each room
    /// </summary>
    public void CreateContent()
    {

        shurfesCount = UnityEngine.Random.Range(shurfesCountRange.Start.Value, shurfesCountRange.End.Value + 1);

        CreateOuterWalls();

        spawnersManager = new();

        //This solution for getting player's reference is for demonstration purpose only. Should be optimized.
        GameObject player = GameObject.FindWithTag("Player");
        EnemyController enemy = spawnableEnemies[UnityEngine.Random.Range(0, spawnableEnemies.Count)];
        spawnersManager.SetSpawners(enemy, transform.position, player.GetComponent<EnemyTargetComponent>(), enemySpawner);

        CreateEntities();
    }

    public void SetContent(IReadOnlyList<EnemyController> enemies, RoomEnemySpawner enemySpawner)
    {
        spawnableEnemies = enemies;
        this.enemySpawner = enemySpawner;
    }

    private void CreateEntities()
    {
        ImmutableList<GameObject>.Builder entitiesBuilder = ImmutableList.CreateBuilder<GameObject>();
        /*
foreach (RoomEntity entity in roomEntities)
{
    Instantiate(entity.GameObject, entity.StartPosition + transform.position, Quaternion.identity, transform);
    entitiesBuilder.Add(entity.GameObject);
}

entities = entitiesBuilder.ToImmutable();
*/
    }


    private void CreateOuterWalls()
    {
        ImmutableList<GameObject>.Builder immutableList = ImmutableList.CreateBuilder<GameObject>();
        for (var i = 0; i < transform.childCount; i++)
        {
            GameObject outerWall = transform.GetChild(i).gameObject;
            if (outerWall.TryGetComponent(out OuterWallBuilder wallBuilder))
                wallBuilder.Create();

            immutableList.Add(outerWall);
        }

        outerWalls = immutableList.ToImmutable();
    }
}
