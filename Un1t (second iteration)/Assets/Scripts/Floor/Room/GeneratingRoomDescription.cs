using UnityEngine;

public class GeneratingRoomDescription
{
    public readonly GameObject RoomInstance;
    public readonly RoomManager.RoomType RoomType;
    public readonly RoomOuterWalls OuterWalls;

    public GeneratingRoomDescription(GameObject roomInstance,
        RoomManager.RoomType roomType, RoomOuterWalls outerWalls)
    {
        RoomInstance = roomInstance;
        RoomType = roomType;
        OuterWalls = outerWalls;
    }
}