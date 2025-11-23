using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UnityEngine;

//TODO: class should be divided according to SRP.

/// <summary>
/// Creates and manages all rooms in the game level
/// </summary>
public class FloorManager : MonoBehaviour
{
    [SerializeField] private FloorEnemiesList spawnableEnemies;

    [SerializeField] private TemplateRoomInfo[] availableCommonRooms;
    [SerializeField] private TemplateRoomInfo[] availableStartRooms;

    [SerializeField] private int minRoomsCount = 5;    //Validate these numbers.
    [SerializeField] private int maxRoomsCount = 7;

    //TODO: next serilize fields should be moved to a separate class.
    [Header("Room types dynamic generation")]
    [SerializeField] private GameObject roomTemplate;

    [SerializeField] private OuterWallBuilder topOuterWall;
    [SerializeField] private OuterWallBuilder bottomOuterWall;
    [SerializeField] private OuterWallBuilder leftOuterWall;
    [SerializeField] private OuterWallBuilder rightOuterWall;

    [SerializeField] private GameObject leftTopCorner;
    [SerializeField] private GameObject rightTopCorner;
    [SerializeField] private GameObject leftBottomCorner;
    [SerializeField] private GameObject rightBottomCorner;

    [SerializeField] private GroundBuilder standardRoomGround;

    [SerializeField] private RoomEnemySpawner enemySpawner;
    [SerializeField] private Rock rock;

    private readonly RoomGrid rooms = new();
    private Dictionary<RoomOuterWalls, ImmutableList<RoomInfo>> groupedRoomsByWalls;

    private void Awake()
    {
        groupedRoomsByWalls = availableCommonRooms
            .GroupBy(room => room.Info.OuterWalls)
            .ToDictionary(
                group => group.Key,
                group => group.Select(template => template.Info).ToImmutableList()
        );

        int roomCount = UnityEngine.Random.Range(minRoomsCount, maxRoomsCount) - 2;
        GenerateFloor(roomCount);
    }

    /// <summary>
    /// Creates all rooms for this level
    /// Each room gets it's own content
    /// </summary>
    private void GenerateFloor(int roomsCount)
    {
        DungeonFactory dungeonFactory = new();
        List<Room> rooms = dungeonFactory.CreateDungeon();

        foreach (Room room in rooms)
        {
            Vector2 roomPosition = (Vector2)((Vector2Int)room.GridPosition * RoomInfo.Size);
            if (TryChooseTemplateRoom(room.OuterWalls, out RoomInfo roomInfo))
                GenerateRoom(roomInfo, room.GridPosition, roomPosition);
            else
                ConstructRoom(room.OuterWalls, room.GridPosition, roomPosition);
        }
    }



    /*
    private void CreateAnotherOneRoom(int roomsCount, in FloorGridPosition roomPositionGrid)
    {
      

        topRoomDescription.Wall ??= RoomOuterWalls.Wall.Solid;
        bottomRoomDescription.Wall ??= RoomOuterWalls.Wall.Solid;
        leftRoomDescription.Wall ??= RoomOuterWalls.Wall.Solid;
        rightRoomDescription.Wall ??= RoomOuterWalls.Wall.Solid;

        RoomOuterWalls outerWalls = new(topRoomDescription.Wall.Value, bottomRoomDescription.Wall.Value, leftRoomDescription.Wall.Value, rightRoomDescription.Wall.Value);

        Vector2 roomPosition = (Vector2)((Vector2Int)roomPositionGrid * RoomInfo.Size);
        if (TryChooseTemplateRoom(outerWalls, out RoomInfo roomInfo))
            GenerateRoom(roomInfo, roomPositionGrid, roomPosition);
        else
            ConstructRoom(outerWalls, roomPositionGrid, roomPosition);

    }
    */
    /// <summary>
    /// Instantiates a room prefab at the specified position
    /// </summary>
    /// <param name="room">Room to instantiate</param>
    /// <param name="gridPosition">Position to place the room</param>
    private void GenerateRoom(RoomInfo room, in FloorGridPosition gridPosition, Vector2 position)
    {
        GameObject roomInstance = Instantiate(room.RoomPrefab, position,
            Quaternion.identity, transform);

        rooms[gridPosition] = room;

        CreateHallwaysGround(position, room.OuterWalls, roomInstance.transform);

        CreateRoomContent(roomInstance);

    }

    /// <summary>
    /// Chooses a room based on the specified outer walls configuration
    /// </summary>
    /// <param name="roomWalls">Outer walls configuration to match</param>
    /// <returns>Matching room or error room if none found</returns>
    private bool TryChooseTemplateRoom(in RoomOuterWalls roomWalls, out RoomInfo roomInfo)
    {
        roomInfo = null;
        if (groupedRoomsByWalls.TryGetValue(roomWalls, out ImmutableList<RoomInfo> possibleRooms)
            && possibleRooms.Count > 0)
        {
            roomInfo = possibleRooms[UnityEngine.Random.Range(0, possibleRooms.Count)];
            return true;
        }
        return false;
    }

    private void CreateRoomContent(GameObject room)
    {
        RoomManager roomManager = room.GetComponent<RoomManager>();
        roomManager.SetContent(spawnableEnemies.Enemies, enemySpawner, rock);
        roomManager.CreateContent();
    }


    //TODO: next methods should be refactored and moved to a separate class,
    //Remove "magic numbers",
    //Caching room types.

    private void ConstructRoom(in RoomOuterWalls roomOuterWalls, in FloorGridPosition gridPosition, Vector2 position)
    {
        GameObject roomInstance = Instantiate(roomTemplate, position, Quaternion.identity, transform);

        Instantiate(topOuterWall, new Vector2(0, 6) + position, Quaternion.identity, roomInstance.transform)
            .GetComponent<StandardOuterWallBuilder>()
            .SetPartsEmptiness(roomOuterWalls.Top);

        Instantiate(bottomOuterWall, new Vector2(0, -5) + position, Quaternion.identity, roomInstance.transform)
            .GetComponent<StandardOuterWallBuilder>()
            .SetPartsEmptiness(roomOuterWalls.Bottom);

        Instantiate(leftOuterWall, new Vector2(-8.5f, 0) + position, Quaternion.identity, roomInstance.transform)
            .GetComponent<StandardOuterWallBuilder>()
            .SetPartsEmptiness(roomOuterWalls.Left);

        Instantiate(rightOuterWall, new Vector2(8.5f, 0) + position, Quaternion.identity, roomInstance.transform)
            .GetComponent<StandardOuterWallBuilder>()
            .SetPartsEmptiness(roomOuterWalls.Right);

        Instantiate(rightBottomCorner, new Vector2(-8.5f, 5) + position, Quaternion.identity, roomInstance.transform);
        Instantiate(leftBottomCorner, new Vector2(8.5f, 5) + position, Quaternion.identity, roomInstance.transform);
        Instantiate(rightTopCorner, new Vector2(-8.5f, -5) + position, Quaternion.identity, roomInstance.transform);
        Instantiate(leftTopCorner, new Vector2(8.5f, -5) + position, Quaternion.identity, roomInstance.transform);

        Instantiate(standardRoomGround, position, Quaternion.identity, roomInstance.transform);

        rooms[gridPosition] = new(roomInstance, roomOuterWalls);
        CreateHallwaysGround(position, roomOuterWalls, roomInstance.transform);

        CreateRoomContent(roomInstance);
    }

    //TODO: universalize this method.
    private void CreateHallwaysGround(Vector2 roomPosition, RoomOuterWalls roomOuterWalls, Transform parent)
    {
        if (roomOuterWalls.Top.Middle.IsEmpty)
            Instantiate(standardRoomGround, roomPosition + new Vector2(0, 6), Quaternion.identity, parent)
            .SetSize(new(6, 3));

        if (roomOuterWalls.Bottom.Middle.IsEmpty)
            Instantiate(standardRoomGround, roomPosition + new Vector2(0, -5), Quaternion.identity, parent)
            .SetSize(new(6, 1));

        if (roomOuterWalls.Left.Middle.IsEmpty)
            Instantiate(standardRoomGround, roomPosition + new Vector2(-8.5f, 0), Quaternion.identity, parent)
            .SetSize(new(1, 3));

        if (roomOuterWalls.Right.Middle.IsEmpty)
            Instantiate(standardRoomGround, roomPosition + new Vector2(8.5f, 0), Quaternion.identity, parent)
            .SetSize(new(1, 3));
    }

}
