using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public static class RoomGroundContentGenerator
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

    public static AllGroundEntities GenerateContent(Tile[,] tiles, GameObject rock, FloorEnemiesList enemies, int enemiesInShurfesCount)
    {
        List<Vector2Int> acceptablePositions = FillAcceptableEntityPositions(tiles);

        int coins = acceptablePositions.Count;

        List<Vector2Int> acceptableRocksPositions = acceptablePositions.ToList();
        List<Vector2Int> acceptableEnemiesPositions = acceptablePositions.ToList();

        foreach (Vector2Int position in UnacceptableRocksPositions)
            acceptableRocksPositions.Remove(position);

        IEnumerable<(GameObject entity, Vector2 startPosition)> rocks = 
            GenerateRocks(coins, acceptablePositions, rock, acceptableEnemiesPositions);

        foreach (Vector2Int position in UnacceptableEnemiesPositions)
            acceptableEnemiesPositions.Remove(position);

        (IEnumerable<(EnemyController entity, Vector2 startPosition)> enemiesOutsideShurfes,
            IReadOnlyList<EnemyController> enemiesInShurfes) =
           GenerateEnemies(coins, acceptableEnemiesPositions, enemies, enemiesInShurfesCount);

        return new(rocks, enemiesOutsideShurfes, enemiesInShurfes);
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
            acceptableEnemyPositions.Remove(rockPosition);

            rocks.Add(new(rock, rockPosition));
        }

        return rocks;
    }

    private static (IEnumerable<(EnemyController entity, Vector2 startPosition)> enemiesOutsideShurfes,
        IReadOnlyList<EnemyController> enemiesInShurfes)
        GenerateEnemies(int coins, List<Vector2Int> acceptablePositions, 
        FloorEnemiesList enemies, int enemiesInShurfesCount)

    {

        EnemyController[] enemiesInShurfes = new EnemyController[enemiesInShurfesCount];

        for (var i = 0; i < enemiesInShurfesCount; i++)
        {
            enemiesInShurfes[i] = EnemySelector.SelectEnemy(enemies);
        }

        List<(EnemyController entity, Vector2 startPosition)> enemiesOutsideShurfes = new();

        int enemiesOutsideShurfesCount = Mathf.RoundToInt(
            math.clamp(UnityEngine.Random.Range(MIN_ENEMIES_FREQUENCY * coins - enemiesInShurfesCount, 
            MAX_ENEMIES_FREQUENCY * coins - enemiesInShurfesCount),
            0, acceptablePositions.Count));

        for (var i = 0; i < enemiesOutsideShurfesCount; i++)
        {
            int enemyPositionNumber = UnityEngine.Random.Range(0, acceptablePositions.Count);
            Vector2Int enemyPosition = acceptablePositions[enemyPositionNumber];
            acceptablePositions.RemoveAt(enemyPositionNumber);

            UnacceptNearPositions(acceptablePositions, enemyPosition);

            enemiesOutsideShurfes.Add((EnemySelector.SelectEnemy(enemies), enemyPosition));

            if (acceptablePositions.Count == 0) break;
        }

        return (enemiesOutsideShurfes, enemiesInShurfes);
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
            unacceptableRocksPositions.Add(new(RoomInfo.Center.x - 1, y));
            unacceptableRocksPositions.Add(new(RoomInfo.Center.x, y));
        }
        for (var x = 0; x < RoomInfo.Center.x; x++)
        {
            unacceptableRocksPositions.Add(new(x, RoomInfo.Center.y - 1));
            unacceptableRocksPositions.Add(new(x, RoomInfo.Center.y));
            unacceptableRocksPositions.Add(new(x, RoomInfo.Center.y + 1));
        }
        for (var x = RoomInfo.Center.x + 1; x < RoomInfo.Size.x; x++)
        {
            unacceptableRocksPositions.Add(new(x, RoomInfo.Center.y - 1));
            unacceptableRocksPositions.Add(new(x, RoomInfo.Center.y));
            unacceptableRocksPositions.Add(new(x, RoomInfo.Center.y + 1));
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
        for (var x = 2; x < RoomInfo.Size.x - 2; x++)
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
    public class AllGroundEntities
    {
        public IEnumerable<(EnemyController entity, Vector2 startPosition)> EnemiesOutsideShurfes
            => enemiesOutsideShurfes;

        public IReadOnlyList<EnemyController> EnemiesInShurfes
            => enemiesInShurfes;

        public IEnumerable<(GameObject entity, Vector2 startPosition)> Rocks
            => rocks;

        private readonly IEnumerable<(GameObject entity, Vector2 startPosition)> rocks;
        private readonly IEnumerable<(EnemyController entity, Vector2 startPosition)> enemiesOutsideShurfes;
        private readonly IReadOnlyList<EnemyController> enemiesInShurfes;

        public AllGroundEntities(IEnumerable<(GameObject entity, Vector2 startPosition)> rocks,
            IEnumerable<(EnemyController entity, Vector2 startPosition)> enemiesOutsideShurfes,
            IReadOnlyList<EnemyController> enemiesInShurfes) 
        { 
            this.rocks = rocks;
            this.enemiesOutsideShurfes = enemiesOutsideShurfes;
            this.enemiesInShurfes = enemiesInShurfes;
        }
    }
    #endregion


}
