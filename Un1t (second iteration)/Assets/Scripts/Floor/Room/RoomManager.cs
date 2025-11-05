using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manages everything inside one room
/// Keeps track of all RoomEntities
/// </summary>
public class RoomManager : MonoBehaviour
{
    public IReadOnlyList<GameObject> Entities => entities;
    public IReadOnlyList<GameObject> OuterWalls => outerWalls;

    private RoomEnemySpawner enemySpawner;
    private Rock rock;

    private SpawnersManager spawnersManager;

    private List<GameObject> entities;
    private List<GameObject> outerWalls;
    private IReadOnlyList<EnemyController> spawnableEnemies;

    private readonly static Range shurfesCountRange = new(2, 5);

    public static int EnemiesCount { get; set; } = 0; //This field is for demo only.

    // TODO: make full room's content generation. This solution is for demonstration purpose only.
    public void CreateContent()
    {

        CreateOuterWalls();

        spawnersManager = new();

        //This solution for getting player's reference is for demonstration purpose only. Should be optimized.
        GameObject player = GameObject.FindWithTag("Player");

        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            EnemyController enemy = spawnableEnemies[UnityEngine.Random.Range(0, spawnableEnemies.Count)];
            spawnersManager.SetSpawners(enemy, transform.position, player.GetComponent<EnemyTargetComponent>(), enemySpawner);
        }
        else
        {
            Instantiate(rock, transform.position, Quaternion.identity);
        }
        CreateEntities();
    }

    public void SetContent(IReadOnlyList<EnemyController> enemies, RoomEnemySpawner enemySpawner, Rock rock)
    {
        spawnableEnemies = enemies;
        this.enemySpawner = enemySpawner;
        this.rock = rock;
    }

    private void CreateEntities()
    {
    }

    //TODO: make full shurf generation, this version is only for demonstration purpose.

    private void CreateOuterWalls()
    {
        List<OuterWallBuilder> shurfableWalls = new();

        List<GameObject> outerWalls = new();
        for (var i = 0; i < transform.childCount; i++)
        {
            GameObject outerWall = transform.GetChild(i).gameObject;
            if (outerWall.TryGetComponent(out TilesBuilder tilesBuilder))
            {
                tilesBuilder.SetConfiguration();
                if (tilesBuilder is OuterWallBuilder wallBuilder
                    && wallBuilder.CanCreateShurf && wallBuilder.Length > 5)
                {
                    int start = wallBuilder.Length / 2;
                    int end = start + 1;
                    wallBuilder.SetShurfesLocation((start, end));
                }

                tilesBuilder.Create();
            }

            outerWalls.Add(outerWall);
        }

    }
}
