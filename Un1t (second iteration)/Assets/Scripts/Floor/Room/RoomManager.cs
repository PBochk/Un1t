using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<EnemyController> AllEnemies => allEnemies;

    private GameObject rock;

    private readonly List<EnemyController> allEnemies = new();

    private FloorEnemiesList spawnableEnemies;
    private IReadOnlyList<TilesBuilder> tilesBuilders;
    private IReadOnlyList<OuterWallBuilder> shurfableWalls;
    private IEnumerable<OuterWallBuilder> wallsWithShurfes;

    private RoomGroundContentGenerator.AllGroundEntities allGroundEntities;

    private Tile[,] tileGrid;

    public void Initialize(FloorEnemiesList enemies, GameObject rock)
    {
        spawnableEnemies = enemies;
        this.rock = rock;
    }

    public void CreateContent()
    {


        ReadTilesBuilders();

        int generatedShurfesCount =
            GenerateShurfes(shurfableWalls);

        GameObject player = GameObject.FindWithTag("Player");
        allGroundEntities = RoomGroundContentGenerator.GenerateContent(tileGrid, rock, spawnableEnemies, generatedShurfesCount);

        CreateEntities(player.GetComponent<EnemyTargetComponent>());
    }

    private void CreateEntities(EnemyTargetComponent player)
    {
        foreach (TilesBuilder tilesBuilder in tilesBuilders)
            tilesBuilder.Create();

        foreach ((GameObject entity, Vector2 startPosition) in allGroundEntities.Rocks)
        {
            Instantiate(entity,
                (Vector3)startPosition + transform.position - (Vector3Int)RoomInfo.Center + (Vector3)Vector2.one / 2,
                Quaternion.identity, transform);
        }

        foreach ((EnemyController entity, Vector2 startPosition) in allGroundEntities.EnemiesOutsideShurfes)
        {
            EnemyController enemy = Instantiate(entity,
                (Vector3)startPosition + transform.position - (Vector3Int)RoomInfo.Center + (Vector3)Vector2.one / 2,
                Quaternion.identity, transform);
            enemy.SetTarget(player);

            allEnemies.Add(enemy);
        }

        if (allGroundEntities.EnemiesInShurfes.Count == 0) return;

        foreach (OuterWallBuilder wallWithShurf in wallsWithShurfes)
        {
            foreach (EnemyController enemy in wallWithShurf.CreateEnemiesInShurfes(allGroundEntities.EnemiesInShurfes))
            {
                enemy.SetTarget(player);
                allEnemies.Add(enemy);
            }
        }
    }

    private void ReadTilesBuilders()
    {
        List<OuterWallBuilder> shurfableWalls = new();
        List<GroundBuilder> groundBuilders = new();
        List<OuterWallBuilder> outerWallBuilders = new();
        List<TilesBuilder> tilesBuilders = new();

        for (var i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.TryGetComponent(out TilesBuilder tilesBuilder))
            {
                tilesBuilder.SetConfiguration();
                if (tilesBuilder is OuterWallBuilder wallBuilder)
                {
                    outerWallBuilders.Add(wallBuilder);
                    if (wallBuilder.CanCreateShurf)
                    {
                        shurfableWalls.Add(wallBuilder);
                    }
                }
                else if (tilesBuilder is GroundBuilder groundBuilder)
                {
                    groundBuilders.Add(groundBuilder);
                }
                tilesBuilders.Add(tilesBuilder);
            }
        }

        this.tilesBuilders = tilesBuilders;
        this.shurfableWalls = shurfableWalls;
        tileGrid = TileConverter.GetTileGrid(groundBuilders, outerWallBuilders, transform.position - (Vector3Int)RoomInfo.Center);
    }

    private int GenerateShurfes(IReadOnlyList<OuterWallBuilder> shurfableWalls)
    {
        List<OuterWallBuilder> wallsWithShurfes = new();

        IReadOnlyDictionary<OuterWallBuilder, List<ShurfEmptyTilesPair>> generatingShurfes =
            ShurfesGenerator.SelectShurfesPositions(shurfableWalls);

        int generatedShurfesCount = 0;
        foreach (OuterWallBuilder wall in generatingShurfes.Keys)
        {
            wall.SetShurfesLocation(generatingShurfes[wall]);
            wallsWithShurfes.Add(wall);
            generatedShurfesCount += generatingShurfes[wall].Count;
        }
        this.wallsWithShurfes = wallsWithShurfes;

        return generatedShurfesCount;
    }

}