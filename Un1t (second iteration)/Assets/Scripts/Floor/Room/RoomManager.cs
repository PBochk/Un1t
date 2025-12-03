using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public IReadOnlyList<GameObject> Entities => entities;
    public IReadOnlyList<GameObject> OuterWalls => outerWalls;

    private RoomEnemySpawner enemySpawner;
    private Rock rock;

    private SpawnersManager spawnersManager;
    private RoomManager roomManager;

    private List<GameObject> entities;
    private List<GameObject> outerWalls;
    private IReadOnlyList<EnemyController> spawnableEnemies;

    private readonly static Range shurfesCountRange = new(2, 5);

    public static int EnemiesCount { get; set; } = 0;

    public void CreateContent(Transform parent)
    {
        CreateOuterWalls(transform.position);

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

    private void CreateOuterWalls(Vector3 roomPosition)
    {
        List<OuterWallBuilder> shurfableWalls = new();
        List<GroundBuilder> groundBuilders = new();
        List<OuterWallBuilder> outerWallBuilders = new();

        List<GameObject> outerWalls = new();
        for (var i = 0; i < transform.childCount; i++)
        {
            GameObject outerWall = transform.GetChild(i).gameObject;
            if (outerWall.TryGetComponent(out TilesBuilder tilesBuilder))
            {
                tilesBuilder.SetConfiguration();
                if (tilesBuilder is OuterWallBuilder wallBuilder)
                {
                    outerWallBuilders.Add(wallBuilder);
                    if (wallBuilder.CanCreateShurf && wallBuilder.Length > 4)
                    {
                        int start = wallBuilder.Length / 2;
                        int end = start + 1;
                        //wallBuilder.SetShurfesLocation((start, end));
                    }
                }
                else if (tilesBuilder is GroundBuilder groundBuilder)
                {
                    groundBuilders.Add(groundBuilder);
                }

                tilesBuilder.Create();
            }

            outerWalls.Add(outerWall);
        }

        Tile[,] tileGrid = GetTileGrid(groundBuilders, outerWallBuilders, roomPosition);
        DrawTileMap(tileGrid);
    }

    private static Tile[,] GetTileGrid(IEnumerable<GroundBuilder> groundBuilders, IEnumerable<OuterWallBuilder> outerWallBuilders, Vector3 roomPosition)
    {
        Tile[,] allTiles = new Tile[RoomInfo.Size.y, RoomInfo.Size.x];

        Vector3 gridOffset = new Vector3(RoomInfo.Size.x, RoomInfo.Size.y) / 2f;

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
                else
                {
                    Debug.LogWarning($"({x}, {y}) is out of tile grid");
                }
            }
        }
    }

    private void DrawTileMap(Tile[,] tiles)
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
}
