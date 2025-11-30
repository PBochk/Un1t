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

    [SerializeField] private GameObject topOuterWall;
    [SerializeField] private GameObject bottomOuterWall;
    [SerializeField] private GameObject leftOuterWall;
    [SerializeField] private GameObject rightOuterWall;


    [SerializeField] private GroundBuilder standardRoomGround;

    [SerializeField] private RoomEnemySpawner enemySpawner;
    [SerializeField] private Rock rock;

    private readonly RoomGrid roomGrid = new();
    private readonly DungeonFactory dungeonFactory = new();

    private Dictionary<RoomOuterWalls, ImmutableList<RoomInfo>> groupedRoomsByWalls;

    private void Awake()
    {
        groupedRoomsByWalls = availableCommonRooms
            .GroupBy(room => room.Info.OuterWalls)
            .ToDictionary(
                group => group.Key,
                group => group.Select(template => template.Info).ToImmutableList()
        );

        GenerateFloor();
    }

    /// <summary>
    /// Creates all rooms for this floor
    /// </summary>
    public void GenerateFloor()
    {
        List<DungeonFactory.Room> rooms = dungeonFactory.CreateDungeon();

        foreach (DungeonFactory.Room room in rooms)
        {
            Vector2 roomPosition = (Vector2)((Vector2Int)room.GridPosition * RoomInfo.Size);
            if (TryChooseTemplateRoom(room.OuterWalls, out RoomInfo roomInfo))
                GenerateRoom(roomInfo, room.GridPosition, roomPosition);
            else
                ConstructRoom(room.OuterWalls, room.GridPosition, roomPosition);
        }

        transform.position -= (Vector3Int)RoomInfo.Size * RoomGrid.FLOOR_SIZE / 2;
    }


    /// <summary>
    /// Instantiates a room prefab at the specified position
    /// </summary>
    /// <param name="room">Room to instantiate</param>
    /// <param name="gridPosition">Position to place the room</param>
    private void GenerateRoom(RoomInfo room, in FloorGridPosition gridPosition, Vector2 position)
    {
        GameObject roomInstance = Instantiate(room.RoomPrefab, position,
            Quaternion.identity, transform);

        roomGrid[gridPosition] = room;

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

        Instantiate(topOuterWall, new Vector2(-3.5f, 7) + position, Quaternion.identity, roomInstance.transform);

        Instantiate(bottomOuterWall, new Vector2(0, -6.5f) + position, Quaternion.identity, roomInstance.transform);

        Instantiate(leftOuterWall, new Vector2(-9.5f, 1) + position, Quaternion.identity, roomInstance.transform);

        Instantiate(rightOuterWall, new Vector2(9.5f, 1) + position, Quaternion.identity, roomInstance.transform);

        roomGrid[gridPosition] = new(roomInstance, roomOuterWalls);
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
