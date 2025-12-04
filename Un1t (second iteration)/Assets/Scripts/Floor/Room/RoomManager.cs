using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private RoomEnemySpawner enemySpawner;
    private GameObject rock;

    private SpawnersManager spawnersManager;
    private RoomManager roomManager;

    private IEnumerable<RoomEntity> entities;
    private List<GameObject> outerWalls;
    private IReadOnlyList<EnemyController> spawnableEnemies;
    private IEnumerable<TilesBuilder> tilesBuilders;

    private Tile[,] tileGrid;

    private static readonly Vector3 gridOffset = new Vector3(RoomInfo.Size.x, RoomInfo.Size.y) / 2f;

    public static int EnemiesCount { get; set; } = 0;

    public void CreateContent(Transform parent)
    {
        ReadTilesBuilders();

        RoomContentCreator contentCreator = new();
        entities = contentCreator.GenerateContent(tileGrid, rock);
        /*
        spawnersManager = new();

        GameObject player = GameObject.FindWithTag("Player");

        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            EnemyController enemy = spawnableEnemies[UnityEngine.Random.Range(0, spawnableEnemies.Count)];
            spawnersManager.SetSpawners(enemy, transform.position, player.GetComponent<EnemyTargetComponent>(), enemySpawner, parent);
        }
        else
        {
            Instantiate(rock, transform.position, Quaternion.identity, parent);
        }*/
        CreateEntities();
    }

    public void SetContent(IReadOnlyList<EnemyController> enemies, RoomEnemySpawner enemySpawner, GameObject rock)
    {
        spawnableEnemies = enemies;
        this.enemySpawner = enemySpawner;
        this.rock = rock;
    }

    private void CreateEntities()
    {
        foreach (TilesBuilder tilesBuilder in tilesBuilders)
            tilesBuilder.Create();

        foreach (RoomEntity roomEntity in entities)
        {
            Instantiate(roomEntity.GameObject, 
                (Vector3Int)roomEntity.StartPosition + transform.position - gridOffset + (Vector3)Vector2.one/2, 
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
        tileGrid = GetTileGrid(groundBuilders, outerWallBuilders, transform.position);

        Dictionary<OuterWallBuilder, List<(int start, int end)>> generatingShurfes =
            SelectShurfesPositions(shurfableWalls, math.clamp(UnityEngine.Random.Range(0, 3), 0, shurfableWalls.Count));

        foreach(OuterWallBuilder wall in generatingShurfes.Keys)
        {
            wall.SetShurfesLocation(generatingShurfes[wall]);
        }
        //DrawTileMap(tileGrid);
    }

    private Dictionary<OuterWallBuilder, List<(int start, int end)>> SelectShurfesPositions
        (List<OuterWallBuilder> shurfableWalls, int shurfesCount)
    {
        var shuffledShurfableWalls =
            from shurfableWall in shurfableWalls
            orderby UnityEngine.Random.value
            select shurfableWall;

        Dictionary<OuterWallBuilder, List<(int start, int end)>> shurfsPositions = new();

        foreach (OuterWallBuilder wall in shuffledShurfableWalls)
        {
            if (wall.Length / OuterWallBuilder.SHURF_WIDTH_WITH_NEIGHBOUR == 0) continue;

            bool[] wallTilesAreEmpty = new bool[wall.Length];

            shurfsPositions[wall] = new List<(int start, int end)>();
            int availableSlots = (wall.Length - OuterWallBuilder.SHURF_WIDTH_WITH_NEIGHBOUR) 
                / OuterWallBuilder.SHURF_WIDTH_WITH_NEIGHBOUR + 1;
            int possiblePairs = math.min(shurfesCount, availableSlots);

            var placedPairs = 0;
            for (var i = 1; i < wall.Length-1 && placedPairs < possiblePairs; i += OuterWallBuilder.SHURF_WIDTH_WITH_NEIGHBOUR)
            {
                if (i + OuterWallBuilder.SHURF_WIDTH <= wall.Length)
                {
                    wallTilesAreEmpty[i] = true;
                    wallTilesAreEmpty[i + 1] = true;

                    shurfsPositions[wall].Add((i, i+1));
                    Debug.Log($"({i}, {i+1})");
                    placedPairs++;
                    shurfesCount--;

                    if (shurfesCount == 0) break;
                }
            }

            if (shurfesCount == 0) break;
        }

        return shurfsPositions;
    }

    #region TileConvertion
    private static Tile[,] GetTileGrid(IEnumerable<GroundBuilder> groundBuilders, IEnumerable<OuterWallBuilder> outerWallBuilders, Vector3 roomPosition)
    {
        Tile[,] allTiles = new Tile[RoomInfo.Size.y, RoomInfo.Size.x];

        foreach (GroundBuilder groundBuilder in groundBuilders)
        {
            Vector3 center = groundBuilder.transform.position - roomPosition + gridOffset;
            Vector2Int size = groundBuilder.SizeTiles;

            FillTileArea(allTiles, center, size, Tile.Ground);
        }

        foreach (OuterWallBuilder wallBuilder in outerWallBuilders)
        {
            wallBuilder.SetConfiguration();

            Vector3 center = wallBuilder.transform.position - roomPosition + gridOffset;
            Vector2Int size = wallBuilder.SizeTiles;

            Tile wallTileType = wallBuilder.CanCreateShurf ? Tile.ShurfableWall : Tile.UnshurfableWall;

            FillTileArea(allTiles, center, size, wallTileType);
        }

        return allTiles;
    }

    private static void FillTileArea(Tile[,] tiles, Vector3 center, Vector2Int size, Tile tileType)
    {
        int startX = Mathf.FloorToInt(center.x - size.x / 2f);
        int endX = startX + size.x - 1;

        int startY = Mathf.FloorToInt(center.y - size.y / 2f);
        int endY = startY + size.y - 1;

        for (var y = startY; y <= endY; y++)
        {
            for (var x = startX; x <= endX; x++)
            {
                if (x >= 0 && x < tiles.GetLength(1) && y >= 0 && y < tiles.GetLength(0))
                {
                    tiles[y, x] = tileType;
                }
            }
        }
    }

    /// <summary>
    /// For debug purpose only
    /// </summary>
    private static void DrawTileMap(Tile[,] tiles)
    {
        StringBuilder sb = new StringBuilder();

        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                Tile tileType = tiles[x, y];
                switch (tileType)
                {
                    case Tile.Ground:
                        sb.Append('G');
                        break;
                    case Tile.ShurfableWall:
                        sb.Append('B');
                        break;
                    case Tile.UnshurfableWall:
                        sb.Append('D');
                        break;
                    default:
                        sb.Append('O');
                        break;
                }
            }
            sb.AppendLine();
        }

        Debug.Log(sb.ToString());
    }
    #endregion
}
