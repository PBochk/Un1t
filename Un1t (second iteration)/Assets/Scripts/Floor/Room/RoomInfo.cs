using UnityEngine;

/// <summary>
/// Represents information about a room including its prefab, outer walls configuration and exits
/// </summary>
public class RoomInfo
{

    public readonly static Vector2Int SIZE = new Vector2Int(16, 9) + new Vector2Int(2, 2);

    public GameObject RoomPrefab { get; }
    public RoomOuterWalls OuterWalls { get; }
    public RoomExits RoomExits { get; }


    public RoomInfo(GameObject roomPrefab,
        bool leftTopIsEmpty, bool middleTopIsEmpty, bool rightTopIsEmpty,
        bool leftBottomIsEmpty, bool middleBottomIsEmpty, bool rightBottomIsEmpty,
        bool topLeftIsEmpty, bool middleLeftIsEmpty, bool bottomLeftIsEmpty,
        bool topRightIsEmpty, bool middleRightIsEmpty, bool bottomRightIsEmpty)
    {
        RoomPrefab = roomPrefab;

        OuterWalls = new(
        new RoomOuterWalls.Wall(
            new RoomOuterWalls.Wall.WallPart(leftTopIsEmpty),
            new RoomOuterWalls.Wall.WallPart(middleTopIsEmpty),
            new RoomOuterWalls.Wall.WallPart(rightTopIsEmpty)
        ),
        new RoomOuterWalls.Wall(
            new RoomOuterWalls.Wall.WallPart(leftBottomIsEmpty),
            new RoomOuterWalls.Wall.WallPart(middleBottomIsEmpty),
            new RoomOuterWalls.Wall.WallPart(rightBottomIsEmpty)
        ),
        new RoomOuterWalls.Wall(
            new RoomOuterWalls.Wall.WallPart(topLeftIsEmpty),
            new RoomOuterWalls.Wall.WallPart(middleLeftIsEmpty),
            new RoomOuterWalls.Wall.WallPart(bottomLeftIsEmpty)
        ),
        new RoomOuterWalls.Wall(
            new RoomOuterWalls.Wall.WallPart(topRightIsEmpty),
            new RoomOuterWalls.Wall.WallPart(middleRightIsEmpty),
            new RoomOuterWalls.Wall.WallPart(bottomRightIsEmpty)
        ));

        RoomExits = CalculateRoomExits(OuterWalls);
    }

    public RoomInfo(GameObject roomPrefab, RoomOuterWalls outerWalls)
    {
        RoomPrefab = roomPrefab;
        OuterWalls = outerWalls;

        RoomExits = CalculateRoomExits(OuterWalls);
    }



    /// <summary>
    /// Calculates room exits based on the outer walls configuration
    /// </summary>
    /// <param name="roomOuterWalls">Outer walls configuration to analyze</param>
    /// <returns>Calculated room exits</returns>
    private static RoomExits CalculateRoomExits(RoomOuterWalls roomOuterWalls)
    {
        static bool CheckWallExit(RoomOuterWalls.Wall wall) =>
            wall.First.IsEmpty || wall.Middle.IsEmpty || wall.Last.IsEmpty;

        return new(CheckWallExit(roomOuterWalls.Top), CheckWallExit(roomOuterWalls.Bottom),
            CheckWallExit(roomOuterWalls.Left), CheckWallExit(roomOuterWalls.Right));
    }
}