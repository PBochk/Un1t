using UnityEngine;

public class RoomConstructor
{
    private static readonly Vector2 topWallOffset = new(-3.5f, 7f);
    private static readonly Vector2 bottomWallOffset = new(0f, -6.5f);
    private static readonly Vector2 leftWallOffset = new(-9.5f, 1f);
    private static readonly Vector2 rightWallOffset = new(9.5f, 1f);

    private static readonly Vector2 topHallwayOffset = new(0f, 7f);
    private static readonly Vector2 bottomHallwayOffset = new(0f, -7f);
    private static readonly Vector2 leftHallwayOffset = new(-9.5f, 0f);
    private static readonly Vector2 rightHallwayOffset = new(9.5f, 0f);

    public GameObject ConstructRoom(
        GameObject roomTemplate,

        GameObject topOuterWall,
        GameObject bottomOuterWall,
        GameObject leftOuterWall,
        GameObject rightOuterWall,

        in RoomOuterWalls roomOuterWalls,
        in FloorGridPosition gridPosition,
        Vector2 position,
        Transform parentTransform,
        RoomGrid roomGrid)
    {
        GameObject roomInstance = GameObject.Instantiate(roomTemplate, position, Quaternion.identity, parentTransform);

        if (!roomOuterWalls.Top.Middle.IsEmpty)
            GameObject.Instantiate(topOuterWall, topWallOffset + position, Quaternion.identity, roomInstance.transform);

        if (!roomOuterWalls.Bottom.Middle.IsEmpty)
            GameObject.Instantiate(bottomOuterWall, bottomWallOffset + position, Quaternion.identity, roomInstance.transform);

        if (!roomOuterWalls.Left.Middle.IsEmpty)
            GameObject.Instantiate(leftOuterWall, leftWallOffset + position, Quaternion.identity, roomInstance.transform);

        if (!roomOuterWalls.Right.Middle.IsEmpty)
            GameObject.Instantiate(rightOuterWall, rightWallOffset + position, Quaternion.identity, roomInstance.transform);

        roomGrid[gridPosition] = new(roomInstance, roomOuterWalls);

        return roomInstance;
    }

    public void CreateHallways(
        GameObject verticalHallway,
        GameObject horizontalHallway,

        Vector2 roomPosition,
        RoomOuterWalls roomOuterWalls,
        Transform parent)
    {
        if (roomOuterWalls.Top.Middle.IsEmpty)
            GameObject.Instantiate(verticalHallway, roomPosition + topHallwayOffset, Quaternion.identity, parent);

        if (roomOuterWalls.Bottom.Middle.IsEmpty)
            GameObject.Instantiate(verticalHallway, roomPosition + bottomHallwayOffset, Quaternion.identity, parent);

        if (roomOuterWalls.Left.Middle.IsEmpty)
            GameObject.Instantiate(horizontalHallway, roomPosition + leftHallwayOffset, Quaternion.identity, parent);

        if (roomOuterWalls.Right.Middle.IsEmpty)
            GameObject.Instantiate(horizontalHallway, roomPosition + rightHallwayOffset, Quaternion.identity, parent);
    }
}