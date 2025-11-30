using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

//TODO: class should be divided according to SRP.

/// <summary>
/// Creates and manages all rooms in the game level
/// </summary>
public class FloorManager : MonoBehaviour
{
    [SerializeField] private FloorEnemiesList spawnableEnemies;

    [SerializeField] private TemplateRoomInfo[] availableCommonRooms;

    //TODO: next serilize fields should be moved to a separate class.
    [Header("Room types dynamic generation")]
    [SerializeField] private GameObject roomTemplate;

    [SerializeField] private GameObject horizontalHallway;
    [SerializeField] private GameObject verticalHallway;

    [SerializeField] private GameObject topOuterWall;
    [SerializeField] private GameObject bottomOuterWall;
    [SerializeField] private GameObject leftOuterWall;
    [SerializeField] private GameObject rightOuterWall;


    [SerializeField] private RoomEnemySpawner enemySpawner;
    [SerializeField] private Rock rock;

    private readonly RoomGrid roomGrid = new();
    private readonly DungeonFactory dungeonFactory = new();
    private IEnumerable<DungeonFactory.Room> allRooms;

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
        allRooms = dungeonFactory.CreateDungeon();

        foreach (DungeonFactory.Room room in allRooms)
        {
            GameObject roomInstance;
            Vector2 roomPosition = (Vector2)((Vector2Int)room.GridPosition * RoomInfo.Size);
            if (TryChooseTemplateRoom(room.OuterWalls, out RoomInfo roomInfo))
                roomInstance = GenerateRoom(roomInfo, room.GridPosition, roomPosition);
            else
                roomInstance = ConstructRoom(room.OuterWalls, room.GridPosition, roomPosition);

            CreateHallways(roomPosition, room.OuterWalls, roomInstance.transform);
            Debug.Log($"({room.GridPosition.X}, {room.GridPosition.Y}) - " +
                $"TOP - {room.OuterWalls.Top.Middle.IsEmpty} " +
                $"BOTTOM - {room.OuterWalls.Bottom.Middle.IsEmpty} " +
                $"LEFT - {room.OuterWalls.Left.Middle.IsEmpty} " +
                $"RIGHT - {room.OuterWalls.Right.Middle.IsEmpty} ");
        }

        transform.position -= (Vector3Int)RoomInfo.Size * RoomGrid.FLOOR_SIZE / 2;
    }


    /// <summary>
    /// Instantiates a room prefab at the specified position
    /// </summary>
    /// <param name="room">Room to instantiate</param>
    /// <param name="gridPosition">Position to place the room</param>
    private GameObject GenerateRoom(RoomInfo room, in FloorGridPosition gridPosition, Vector2 position)
    {
        GameObject roomInstance = Instantiate(room.RoomPrefab, position,
            Quaternion.identity, transform);

        roomGrid[gridPosition] = room;

        CreateRoomContent(roomInstance);

        return roomInstance;
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

    private GameObject ConstructRoom(in RoomOuterWalls roomOuterWalls, in FloorGridPosition gridPosition, Vector2 position)
    {
        GameObject roomInstance = Instantiate(roomTemplate, position, Quaternion.identity, transform);

        if (!roomOuterWalls.Top.Middle.IsEmpty)
            Instantiate(topOuterWall, new Vector2(-3.5f, 7) + position, Quaternion.identity, roomInstance.transform);

        if (!roomOuterWalls.Bottom.Middle.IsEmpty)
            Instantiate(bottomOuterWall, new Vector2(0, -6.5f) + position, Quaternion.identity, roomInstance.transform);

        if (!roomOuterWalls.Left.Middle.IsEmpty)
            Instantiate(leftOuterWall, new Vector2(-9.5f, 1) + position, Quaternion.identity, roomInstance.transform);

        if (!roomOuterWalls.Right.Middle.IsEmpty)
            Instantiate(rightOuterWall, new Vector2(9.5f, 1) + position, Quaternion.identity, roomInstance.transform);

        roomGrid[gridPosition] = new(roomInstance, roomOuterWalls);

        CreateRoomContent(roomInstance);

        return roomInstance;
    }

    private void CreateHallways(Vector2 roomPosition, RoomOuterWalls roomOuterWalls, Transform parent)
    {
        if (roomOuterWalls.Top.Middle.IsEmpty)
            Instantiate(verticalHallway, roomPosition + new Vector2(0, 7), Quaternion.identity, parent);

        if (roomOuterWalls.Bottom.Middle.IsEmpty)
            Instantiate(verticalHallway, roomPosition + new Vector2(0, -7), Quaternion.identity, parent);

        if (roomOuterWalls.Left.Middle.IsEmpty)
            Instantiate(horizontalHallway, roomPosition + new Vector2(-9.5f, 0), Quaternion.identity, parent);

        if (roomOuterWalls.Right.Middle.IsEmpty)
            Instantiate(horizontalHallway, roomPosition + new Vector2(9.5f, 0), Quaternion.identity, parent);
    }

}
