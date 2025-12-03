using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RoomContentCreator
{

    private readonly int minRocksCount = 0;
    private readonly int maxRocksCount = 10;

    private  IEnumerable<Vector2Int>
        unacceptableRockPositions = null;

    private static readonly int roomCenterX = RoomInfo.Size.x / 2;
    private static readonly int roomCenterY = RoomInfo.Size.y / 2;

    public IEnumerable<RoomEntity> GenerateContent(Tile[,] tiles, GameObject rock)
    {
        List<RoomEntity> roomEntities = new();

        unacceptableRockPositions = FillUnacceptableRockPositions(tiles);
        List<Vector2Int> acceptableRockPositions = FillAcceptableRockPositions(tiles);

        if (acceptableRockPositions.Count == 0) return null;

        int rocksCount = math.clamp(UnityEngine.Random.Range(minRocksCount, maxRocksCount), 
            minRocksCount, maxRocksCount);

        for (var i = 0; i < rocksCount; i++)
        {
            int rockPositionNumber = UnityEngine.Random.Range(0, acceptableRockPositions.Count);
            Vector2Int rockPosition = acceptableRockPositions[rockPositionNumber];
            acceptableRockPositions.RemoveAt(rockPositionNumber);

            roomEntities.Add(new(rock, rockPosition));
        }

        return roomEntities;
    }

    private List<Vector2Int> FillAcceptableRockPositions(Tile[,] tiles)
    {
        List<Vector2Int> acceptableRockPositions = new();

        for (var y = 0; y < RoomInfo.Size.y; y++)
        {
            for (var x = 0; x < RoomInfo.Size.x; x++)
            {
                if (tiles[y, x] == Tile.Ground)
                    acceptableRockPositions.Add(new(x, y));
            }
        }

        foreach (Vector2Int position in unacceptableRockPositions)
            acceptableRockPositions.Remove(position);

        return acceptableRockPositions;


    }

    private IEnumerable<Vector2Int> FillUnacceptableRockPositions(Tile[,] tiles)
    {
        List<Vector2Int> unacceptableRockPositions = new();

        for (var y = 0; y < RoomInfo.Size.y; y++)
        {
            unacceptableRockPositions.Add(new(roomCenterX - 1, y));
            unacceptableRockPositions.Add(new(roomCenterX, y));
        }
        for (var x = 0; x < roomCenterX; x++)
        {
            unacceptableRockPositions.Add(new(x, roomCenterY-1));
            unacceptableRockPositions.Add(new(x, roomCenterY));
            unacceptableRockPositions.Add(new(x, roomCenterY+1));
        }
        for (var x = roomCenterX + 1; x < RoomInfo.Size.x; x++)
        {
            unacceptableRockPositions.Add(new(x, roomCenterY - 1));
            unacceptableRockPositions.Add(new(x, roomCenterY));
            unacceptableRockPositions.Add(new(x, roomCenterY + 1));
        }

        return unacceptableRockPositions;
    }

}
