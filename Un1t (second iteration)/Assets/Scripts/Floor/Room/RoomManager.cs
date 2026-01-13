using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> AllEnemies => allEnemies;

    private readonly List<GameObject> allEnemies = new();

    private FloorEnemiesList spawnableEnemies;
    private FloorObjectsList floorObjectsList;

    private IReadOnlyList<TilesBuilder> tilesBuilders;
    private IReadOnlyList<OuterWallBuilder> shurfableWalls;
    private IEnumerable<OuterWallBuilder> wallsWithShurfes;

    private RoomGroundContentGenerator.AllGroundEntities allGroundEntities;

    private Tile[,] tileGrid;

    private RoomType type;     //TODO: refactor room typing according to OCP

    private DoorsConstructor doorsConstructor;

    private RoomCompletionStage completionStage = RoomCompletionStage.Uncleaned;

    public void Initialize(FloorEnemiesList enemies, FloorObjectsList floorObjectsList, DoorsConstructor doorsConstructor)
    {
        spawnableEnemies = enemies;
        this.floorObjectsList = floorObjectsList;
        this.doorsConstructor = doorsConstructor;
    }

    public void CreateContent(RoomType roomType)
    {
        type = roomType;

        ReadTilesBuilders();

        int generatedShurfesCount = roomType == RoomType.Battle
        ? GenerateShurfes(shurfableWalls) : 0;

        allGroundEntities = RoomGroundContentGenerator.GenerateContent(tileGrid, 
            floorObjectsList.Rock, spawnableEnemies, generatedShurfesCount);

        CreateEntities();
    }

    private void CreateEntities()
    {
        foreach (TilesBuilder tilesBuilder in tilesBuilders)
            tilesBuilder.Create();

        if (type == RoomType.Exit)
        {
            Instantiate(floorObjectsList.Descent, transform);
            tileGrid[tileGrid.GetLength(0)/2, tileGrid.GetLength(1)/2] = Tile.ShurfableWall;
            Vector2Int? tentCoordinates = FindSquareCenter(Tile.Ground, 6);

            if (!tentCoordinates.HasValue) return;
            Instantiate(floorObjectsList.Tent,
              (Vector3Int)tentCoordinates.Value + transform.position - (Vector3Int)RoomInfo.Center + (Vector3)Vector2.one / 2,
            Quaternion.identity, transform);
            return;
        }
        else if (type == RoomType.Entrance)
        {
            return;
        }


        foreach (RoomEntity rock in allGroundEntities.Rocks)
        {
            Instantiate(rock.GameObject,
                (Vector3)rock.StartPosition + transform.position - (Vector3Int)RoomInfo.Center + (Vector3)Vector2.one / 2,
                Quaternion.identity, transform);
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
            foreach (RoomEntity enemyEntity in allGroundEntities.EnemiesOutsideShurfes)
            {
                EnemyController enemyController = Instantiate(enemyEntity.GameObject,
                    (Vector3)enemyEntity.StartPosition + transform.position - (Vector3Int)RoomInfo.Center + (Vector3)Vector2.one / 2,
                    Quaternion.identity, transform).GetComponent<EnemyController>();
                enemyController.SetTarget(enemyTarget);

                allEnemies.Add(enemyEntity.GameObject);

                enemyController.Model.OnDeath.AddListener(() => EnemyDead(enemyEntity.GameObject));
            }

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

        void EnemyDead(GameObject enemy)
        {
            allEnemies.Remove(enemy);
            if (allEnemies.Count == 0)
            {
                completionStage = RoomCompletionStage.Cleaned;
                doorsConstructor.DestroyDoors();
            }
        }
}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out PlayerController player)) return;

        if (completionStage != RoomCompletionStage.Uncleaned || type != RoomType.Battle) return;

        completionStage = RoomCompletionStage.Battle;
        CreateEnemies(player.GetComponent<EnemyTargetComponent>());
        doorsConstructor.ConstructDoors(transform);
        
    }

    public enum RoomType : sbyte{ Battle, Entrance, Exit }

    private enum RoomCompletionStage : sbyte { Uncleaned, Battle, Cleaned }

    #region FindEmptySquare
    private Vector2Int? FindSquareCenter(Tile tileType, int squareSize = 6)
    {
        int width = tileGrid.GetLength(0);
        int height = tileGrid.GetLength(1);

        for (int x = 0; x <= width - squareSize; x++)
        {
            for (int y = 0; y <= height - squareSize; y++)
            {
                if (IsSquareOfType(x, y, tileType, squareSize))
                {
                    int centerX = x + squareSize / 2;
                    int centerY = y + squareSize / 2;
                    return new Vector2Int(centerY, centerX);
                }
            }
        }

        return null;
    }


    private bool IsSquareOfType(int startX, int startY, Tile tileType, int size)
    {
        for (int x = startX; x < startX + size; x++)
        {
            for (int y = startY; y < startY + size; y++)
            {
                if (tileGrid[x, y] != tileType)
                {
                    return false;
                }
            }
        }
        return true;
    }
    #endregion

}

