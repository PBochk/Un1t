using UnityEngine;

/// <summary>
/// Represents information about a room including its prefab, outer walls configuration and exits
/// </summary>
public class RoomInfo
{
    public static Vector2Int Size => size;
    public static Vector2Int InnerSize => innerSize;

    public GameObject RoomPrefab { get; }
    public RoomOuterWalls OuterWalls { get; }

    private static readonly Vector2Int innerSize = new(16, 9);
    private static readonly Vector2Int size = innerSize + new Vector2Int(2, 4);

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
    }

    public RoomInfo(GameObject roomPrefab, RoomOuterWalls outerWalls)
    {
        RoomPrefab = roomPrefab;
        OuterWalls = outerWalls;
    }
}