using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> AllEnemies => allEnemies;

    private GameObject rock;
    private GameObject descent;

    private readonly List<GameObject> allEnemies = new();

    private FloorEnemiesList spawnableEnemies;

    private IReadOnlyList<TilesBuilder> tilesBuilders;
    private IReadOnlyList<OuterWallBuilder> shurfableWalls;
    private IReadOnlyList<GameObject> doorWalls;
    private IEnumerable<OuterWallBuilder> wallsWithShurfes;

    private RoomGroundContentGenerator.AllGroundEntities allGroundEntities;

    private Tile[,] tileGrid;

    //TODO: refactor room typing according to OCP
    private DungeonFactory.Room.RoomType type;

    private EnemyTargetComponent player;

    private DoorsConstructor doorsConstructor;

    private RoomCompletionStage completionStage = RoomCompletionStage.Uncleaned;


    public void Initialize(FloorEnemiesList enemies, GameObject rock, GameObject descent,
        EnemyTargetComponent enemyTarget, DoorsConstructor doorsConstructor)
    {
        spawnableEnemies = enemies;
        this.rock = rock;
        this.descent = descent;
        this.doorsConstructor = doorsConstructor;
    }

    public void CreateContent(DungeonFactory.Room.RoomType roomType)
    {
        type = roomType;

        ReadTilesBuilders();

        int generatedShurfesCount = roomType == DungeonFactory.Room.RoomType.Regular
        ? GenerateShurfes(shurfableWalls) : 0;

        allGroundEntities = RoomGroundContentGenerator.GenerateContent(tileGrid, rock, spawnableEnemies, generatedShurfesCount);

        CreateEntities(player);
    }

    private void CreateEntities(EnemyTargetComponent player)
    {
        foreach (TilesBuilder tilesBuilder in tilesBuilders)
            tilesBuilder.Create();

        if (type == DungeonFactory.Room.RoomType.Exit)
            Instantiate(descent, transform);

        if (type != DungeonFactory.Room.RoomType.Regular) return;

        if (allGroundEntities.Rocks.Count != 0)
        {

            foreach (RoomEntity rock in allGroundEntities.Rocks)
            {
                Instantiate(rock.GameObject,
                    (Vector3)rock.StartPosition + transform.position - (Vector3Int)RoomInfo.Center + (Vector3)Vector2.one / 2,
                    Quaternion.identity, transform);
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

    private void CreateEnemies(EnemyTargetComponent enemyTarget)
    {
        if (allGroundEntities.EnemiesOutsideShurfes.Count != 0)
        {
            foreach (RoomEntity enemyEntity in allGroundEntities.EnemiesOutsideShurfes)
            {
                EnemyController enemyController = Instantiate(enemyEntity.GameObject,
                    (Vector3)enemyEntity.StartPosition + transform.position - (Vector3Int)RoomInfo.Center + (Vector3)Vector2.one / 2,
                    Quaternion.identity, transform).GetComponent<EnemyController>();
                enemyController.SetTarget(enemyTarget);

                allEnemies.Add(enemyEntity.GameObject);

                enemyController.Model.OnDeath.AddListener(() => EnemyDead(enemyEntity.GameObject));
            }
        }

        if (allGroundEntities.EnemiesInShurfes.Count == 0) return;

        foreach (OuterWallBuilder wallWithShurf in wallsWithShurfes)
        {
            foreach (GameObject enemy in wallWithShurf.CreateEnemiesInShurfes(allGroundEntities.EnemiesInShurfes))
            {
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                enemyController.SetTarget(enemyTarget);
                allEnemies.Add(enemy);

                enemyController.Model.OnDeath.AddListener(() => EnemyDead(enemy));
            }
        }
    }

    private void EnemyDead(GameObject enemy)
    {
        allEnemies.Remove(enemy);
        if (allEnemies.Count == 0)
        {
            completionStage = RoomCompletionStage.Cleaned;
            doorsConstructor.DestroyDoors();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out PlayerController player)) return;

        if (completionStage != RoomCompletionStage.Uncleaned || type != DungeonFactory.Room.RoomType.Regular) return;

        completionStage = RoomCompletionStage.Battle;
        CreateEnemies(player.GetComponent<EnemyTargetComponent>());
        doorsConstructor.ConstructDoors(transform);
        
    }

    private enum RoomCompletionStage { Uncleaned, Battle, Cleaned }

}