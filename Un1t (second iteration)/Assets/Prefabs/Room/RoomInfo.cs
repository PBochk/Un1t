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
        static bool checkWallExit(RoomOuterWalls.Wall wall) =>
            wall.First.IsEmpty || wall.Middle.IsEmpty || wall.Last.IsEmpty;

        return new(checkWallExit(roomOuterWalls.Top), checkWallExit(roomOuterWalls.Bottom),
            checkWallExit(roomOuterWalls.Left), checkWallExit(roomOuterWalls.Right));
    }




    //TODO: next methods should be refactored and moved to a separate class

    public static RoomInfo ConstructRoom(in RoomOuterWalls roomOuterWalls,
        GameObject roomTemplate, GameObject sideWallPart, GameObject baseWallPart)
    {
        GameObject roomInstance = GameObject.Instantiate(roomTemplate);

        CreateWall(roomInstance.transform, roomOuterWalls.Top, baseWallPart, new Vector2(0-6, 10 - 5), WallDirection.Horizontal);
        CreateWall(roomInstance.transform, roomOuterWalls.Bottom, baseWallPart, new Vector2(0 - 6, 0 - 5), WallDirection.Horizontal);
        CreateWall(roomInstance.transform, roomOuterWalls.Left, sideWallPart, new Vector2(-3f+0.333f - 6, 2 - 5), WallDirection.Vertical);
        CreateWall(roomInstance.transform, roomOuterWalls.Right, sideWallPart, new Vector2(16-0.666f - 6, 2 - 5), WallDirection.Vertical);

        return new RoomInfo(roomInstance, roomOuterWalls);
    }

    private static void CreateWall(Transform parent, in RoomOuterWalls.Wall wall,
        GameObject wallPart, Vector2 startPosition, WallDirection direction)
    {
        Vector2 currentPos = startPosition;
        Vector2 step = direction == WallDirection.Horizontal ? new Vector2(6.333f, 0f) : new Vector2(0, 3);

        if (!wall.First.IsEmpty)
        {
            GameObject partInstance = GameObject.Instantiate(wallPart, parent);
            partInstance.transform.position = currentPos;
        }
        currentPos += step;

        if (!wall.Middle.IsEmpty)
        {
            GameObject partInstance = GameObject.Instantiate(wallPart, parent);
            partInstance.transform.position = currentPos;
        }
        currentPos += step;

        if (!wall.Last.IsEmpty)
        {
            GameObject partInstance = GameObject.Instantiate(wallPart, parent);
            partInstance.transform.position = currentPos;
        }
    }

    private enum WallDirection
    {
        Horizontal, Vertical
    }


}