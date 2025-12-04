using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.Overlays;
using UnityEngine;

public static class RoomContentCreator
{
    private const float MIN_ROCKS_FREQUENCY = 0f;
    private const float MAX_ROCKS_FREQUENCY = 1f/12f;

    private const float MIN_ENEMIES_FREQUENCY = 1f/24f;
    private const float MAX_ENEMIES_FREQUENCY = 7f/120f;

    private static List<Vector2Int> UnacceptableRocksPositions =>
        unacceptableRocksPositions ??= FillUnacceptableRocksPositions();
    private static List<Vector2Int> UnacceptableEnemiesPositions =>
    unacceptableEnemiesPositions ??= FillUnacceptableEnemiesPositions();


    private static List<Vector2Int>
        unacceptableRocksPositions = null;
    private static List<Vector2Int>
         unacceptableEnemiesPositions = null;

    private static readonly int roomCenterX = RoomInfo.Size.x / 2;
    private static readonly int roomCenterY = RoomInfo.Size.y / 2;

    public static AllEntities GenerateContent(Tile[,] tiles, GameObject rock, EnemyController enemy)
    {
            
        List<Vector2Int> acceptablePositions = FillAcceptableEntityPositions(tiles);

        if (acceptablePositions.Count == 0) return null;

        int coins = acceptablePositions.Count;

        List<Vector2Int> acceptableRocksPositions = acceptablePositions.ToList();
        List<Vector2Int> acceptableEnemiesPositions = acceptablePositions.ToList();

        foreach (Vector2Int position in UnacceptableRocksPositions)
            acceptableRocksPositions.Remove(position);

        IEnumerable<(GameObject entity, Vector2 startPosition)> rocks = 
            GenerateRocks(coins, acceptablePositions, rock, acceptableEnemiesPositions);

        foreach (Vector2Int position in UnacceptableEnemiesPositions)
            acceptableEnemiesPositions.Remove(position);

        IEnumerable<(EnemyController entity, Vector2 startPosition)> enemies =
           GenerateEnemies(coins, acceptableEnemiesPositions, enemy);

        return new(rocks, enemies);
    }

    private static IEnumerable<(GameObject entity, Vector2 startPosition)> GenerateRocks(int coins, List<Vector2Int> acceptablePositions, 
        GameObject rock, List<Vector2Int> acceptableEnemyPositions)
    {
        List<(GameObject entity, Vector2 startPosition)> rocks = new();

        int rocksCount = Mathf.RoundToInt(math.clamp(
            UnityEngine.Random.Range(MIN_ROCKS_FREQUENCY * coins, MAX_ROCKS_FREQUENCY * coins),
            0, acceptablePositions.Count));

        for (var i = 0; i < rocksCount; i++)
        {
            int rockPositionNumber = UnityEngine.Random.Range(0, acceptablePositions.Count);
            Vector2Int rockPosition = acceptablePositions[rockPositionNumber];
            acceptablePositions.RemoveAt(rockPositionNumber);

            rocks.Add(new(rock, rockPosition));
        }

        return rocks;
    }

    private static IEnumerable<(EnemyController entity, Vector2 startPosition)> GenerateEnemies(int coins, List<Vector2Int> acceptablePositions, 
        EnemyController enemy)
    {
        List<(EnemyController entity, Vector2 startPosition)> enemies = new();

        int enemiesWithinShurfesCount = Mathf.RoundToInt(
            math.clamp(UnityEngine.Random.Range(MIN_ENEMIES_FREQUENCY * coins, MAX_ENEMIES_FREQUENCY * coins),
            0, acceptablePositions.Count));

        for (var i = 0; i < enemiesWithinShurfesCount; i++)
        {
            int enemyPositionNumber = UnityEngine.Random.Range(0, acceptablePositions.Count);
            Vector2Int enemyPosition = acceptablePositions[enemyPositionNumber];
            acceptablePositions.RemoveAt(enemyPositionNumber);

            UnacceptNearPositions(acceptablePositions, enemyPosition);

            enemies.Add(new(enemy, enemyPosition));

            if (acceptablePositions.Count == 0) break;
        }

        return enemies;
    }

    private static List<Vector2Int> FillAcceptableEntityPositions(Tile[,] tiles)
    {
        List<Vector2Int> acceptableEntitiesPositions = new();

        for (var y = 0; y < RoomInfo.Size.y; y++)
        {
            for (var x = 0; x < RoomInfo.Size.x; x++)
            {
                switch (tiles[y, x])
                {
                    case Tile.Ground:
                        acceptableEntitiesPositions.Add(new(x, y));
                        break;
                }
            }
        }

        return acceptableEntitiesPositions;


    }

    private static List<Vector2Int> UnacceptNearPositions(List<Vector2Int> acceptablePositions, 
        Vector2Int occupiedPosition)
    {
        for (var y = -1 + occupiedPosition.y; y < 2 + occupiedPosition.y; y++)
            for (var x = -1 + occupiedPosition.x; x < 2 + occupiedPosition.x; x++)
                acceptablePositions.Remove(new(x, y));

        return acceptablePositions;
    }

    #region UnacceptablePositionsFilling
    private static List<Vector2Int> FillUnacceptableRocksPositions()
    {
        List<Vector2Int> unacceptableRocksPositions = new();

        for (var y = 0; y < RoomInfo.Size.y; y++)
        {
            unacceptableRocksPositions.Add(new(roomCenterX - 1, y));
            unacceptableRocksPositions.Add(new(roomCenterX, y));
        }
        for (var x = 0; x < roomCenterX; x++)
        {
            unacceptableRocksPositions.Add(new(x, roomCenterY-1));
            unacceptableRocksPositions.Add(new(x, roomCenterY));
            unacceptableRocksPositions.Add(new(x, roomCenterY+1));
        }
        for (var x = roomCenterX + 1; x < RoomInfo.Size.x; x++)
        {
            unacceptableRocksPositions.Add(new(x, roomCenterY - 1));
            unacceptableRocksPositions.Add(new(x, roomCenterY));
            unacceptableRocksPositions.Add(new(x, roomCenterY + 1));
        }

        return unacceptableRocksPositions;
    }

    private static List<Vector2Int> FillUnacceptableEnemiesPositions()
    {
        List<Vector2Int> unacceptableEnemiesPositions = new();

        for (var y = 0; y < RoomInfo.Size.y; y++)
        {
            unacceptableEnemiesPositions.Add(new(0, y));
            unacceptableEnemiesPositions.Add(new(1, y));
            unacceptableEnemiesPositions.Add(new(2, y));

            unacceptableEnemiesPositions.Add(new(RoomInfo.Size.x, y));
            unacceptableEnemiesPositions.Add(new(RoomInfo.Size.x-1, y));
            unacceptableEnemiesPositions.Add(new(RoomInfo.Size.x-2, y));
        }
        for (var x = 2; x < roomCenterX-2; x++)
        {
            unacceptableEnemiesPositions.Add(new(x, 0));
            unacceptableEnemiesPositions.Add(new(x, 1));
            unacceptableEnemiesPositions.Add(new(x, 2));

            unacceptableEnemiesPositions.Add(new(x, RoomInfo.Size.y));
            unacceptableEnemiesPositions.Add(new(x, RoomInfo.Size.y - 1));
            unacceptableEnemiesPositions.Add(new(x, RoomInfo.Size.y - 2));
        }

        return unacceptableEnemiesPositions;
    }
    #endregion

    #region AllEntities
    public class AllEntities
    {
        public IEnumerable<(GameObject entity, Vector2 startPosition)> Rocks { get; }
        public IEnumerable<(EnemyController entity, Vector2 startPosition)> Enemies { get; }

        public AllEntities(IEnumerable<(GameObject entity, Vector2 startPosition)> rocks,
            IEnumerable<(EnemyController entity, Vector2 startPosition)> enemies) 
        { 
            Rocks = rocks;
            Enemies = enemies;
        }
    }
    #endregion


}
