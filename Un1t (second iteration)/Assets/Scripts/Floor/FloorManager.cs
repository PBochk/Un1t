using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

//TODO: class should be divided according to SRP.

/// <summary>
/// Creates and manages all rooms in the game level
/// </summary>
public class FloorManager : MonoBehaviour
{
    private Dictionary<RoomOuterWalls, IList<RoomInfo>> GroupedRoomsByWall => 
        groupedRoomsByWalls ??= (from room in availableCommonRooms
                                 group room by room.Info.OuterWalls)
            .ToDictionary(
                group => group.Key,
                group => group.Select(template => template.Info).AsReadOnlyList()
        );

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

    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject descent;

    //It's temporary implementation for demo only
    [SerializeField] private GameObject doorWall;


    private readonly RoomGrid roomGrid = new();
    private readonly DungeonFactory dungeonFactory = new();
    private IEnumerable<(GameObject roomInstance, DungeonFactory.Room.RoomType roomType, RoomOuterWalls outerWalls)> allRooms; //It's temporary implementation for demo only

    private Dictionary<RoomOuterWalls, IList<RoomInfo>> groupedRoomsByWalls;

    private GameObject player;


    /// <summary>
    /// Creates all rooms for this floor
    /// </summary>
    public void GenerateFloor()
    {
        List<(GameObject roomInstance, DungeonFactory.Room.RoomType roomType, RoomOuterWalls outerWalls)> roomInstances = new();     //It's temporary implementation for demo only
        foreach (DungeonFactory.Room room in dungeonFactory.CreateDungeon())
        {
            GameObject roomInstance;
            Vector2 roomPosition = (Vector2)((Vector2Int)room.GridPosition * RoomInfo.Size);
            if (TryChooseTemplateRoom(room.OuterWalls, out RoomInfo roomInfo))
                roomInstance = GenerateRoom(roomInfo, room.GridPosition, roomPosition);
            else
                roomInstance = ConstructRoom(room.OuterWalls, room.GridPosition, roomPosition);

            CreateHallways(roomPosition, room.OuterWalls, roomInstance.transform);
            roomInstances.Add((roomInstance, room.Type, room.OuterWalls));
        }
        allRooms = roomInstances;
        transform.position -= (Vector3Int)RoomInfo.Size * RoomGrid.FLOOR_SIZE / 2;
    }

    public void GenerateRoomsContent()
    {
        foreach ((GameObject roomInstance, DungeonFactory.Room.RoomType roomType, RoomOuterWalls outerWalls) in allRooms)
            CreateRoomContent(roomInstance, roomType, outerWalls);
    }

    public void SetPlayer(GameObject enemyTarget)
    {
        player = enemyTarget;
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
        if (GroupedRoomsByWall.TryGetValue(roomWalls, out IList<RoomInfo> possibleRooms)
            && possibleRooms.Count > 0)
        {
            roomInfo = possibleRooms[Random.Range(0, possibleRooms.Count)];
            return true;
        }
        return false;
    }

    private void CreateRoomContent(GameObject room, DungeonFactory.Room.RoomType roomType, RoomOuterWalls outerWalls)
    {
        RoomManager roomManager = room.GetComponent<RoomManager>();
        roomManager.Initialize(spawnableEnemies, rock, descent, player.GetComponent<EnemyTargetComponent>(), doorWall, outerWalls);
        roomManager.CreateContent(roomType);
    }


    //TODO: next methods should be refactored and moved to a separate class,
    //Remove "magic numbers",

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
