using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RoomContentCreator
{
    private const int MIN_ROCKS_COUNT = 0;
    private const int MAX_ROCKS_COUNT = 10;

    private const int MIN_SHURFES_COUNT = 0;
    private const int MAX_SHURFES_COUNT = 3;

    private IEnumerable<Vector2Int>
        unacceptableRockPositions = null;

    private static readonly int roomCenterX = RoomInfo.Size.x / 2;
    private static readonly int roomCenterY = RoomInfo.Size.y / 2;

    public IEnumerable<RoomEntity> GenerateContent(Tile[,] tiles, GameObject rock)
    {
        List<RoomEntity> roomEntities = new();

        unacceptableRockPositions = FillUnacceptableRockPositions(tiles);
        List<Vector2Int> acceptableRockPositions = FillAcceptableEntityPositions(tiles);

        if (acceptableRockPositions.Count == 0) return null;

        int rocksCount = math.clamp(UnityEngine.Random.Range(MIN_ROCKS_COUNT, MAX_ROCKS_COUNT), 
            0, acceptableRockPositions.Count);

        for (var i = 0; i < rocksCount; i++)
        {
            int rockPositionNumber = UnityEngine.Random.Range(0, acceptableRockPositions.Count);
            Vector2Int rockPosition = acceptableRockPositions[rockPositionNumber];
            acceptableRockPositions.RemoveAt(rockPositionNumber);

            roomEntities.Add(new(rock, rockPosition));
        }

        return roomEntities;
    }

    private List<Vector2Int> FillAcceptableEntityPositions(Tile[,] tiles)
    {
        List<Vector2Int> acceptableRockPositions = new();

        for (var y = 0; y < RoomInfo.Size.y; y++)
        {
            for (var x = 0; x < RoomInfo.Size.x; x++)
            {
                switch (tiles[y, x])
                {
                    case Tile.Ground:
                        acceptableRockPositions.Add(new(x, y));
                        break;

                    case Tile.ShurfableWall:
                        //acceptableRockPositions.Add(new(x, y));
                        break;
                }
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
