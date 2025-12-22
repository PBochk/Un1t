using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] private FloorTheme floorTheme = FloorTheme.Light;

    [SerializeField, Tooltip("Maximum distance between entarance room and exit room")] 
        private int minDistance = 5;
    [SerializeField, Tooltip("Minimum distance between entarance room and exit room")]
        private int maxDistance = 7;

    [SerializeField] private FloorObjectsList floorObjectsList;
    [SerializeField] private FloorEnemiesList spawnableEnemies;

    [SerializeField] private TemplateRoomInfo[] availableCommonRooms;

    private readonly RoomGrid roomGrid = new();
    private readonly DungeonFactory dungeonFactory = new();
    private IEnumerable<GeneratingRoomDescription> allRooms;

    private Dictionary<RoomOuterWalls, IList<RoomInfo>> groupedRoomsByWalls;

    private void Awake()
    {
        FloorThemeManager.CurrentTheme = floorTheme;
    }

    /// <summary>
    /// Creates all rooms for this floor
    /// </summary>
    public void GenerateFloor()
    {
        StandardRoomConstructor roomConstructor = new(floorObjectsList);

        groupedRoomsByWalls =
            (from room in availableCommonRooms
             group room by room.Info.OuterWalls)
            .ToDictionary(
                group => group.Key,
                group => group.Select(template => template.Info).AsReadOnlyList()
        );

        List<GeneratingRoomDescription> roomInstances = new();

        foreach (DungeonFactory.Room room in dungeonFactory.CreateDungeon(minDistance: minDistance, maxDistance: maxDistance))
        {
            GameObject roomInstance;
            Vector2 roomPosition = (Vector2)((Vector2Int)room.GridPosition * RoomInfo.Size);
            if (TryChooseTemplateRoom(room.OuterWalls, out RoomInfo roomInfo))
                roomInstance = GenerateRoom(roomInfo, room.GridPosition, roomPosition);
            else
                roomInstance = roomConstructor.ConstructRoom(
                    room.OuterWalls,
                    room.GridPosition,
                    roomPosition,
                    transform,
                    roomGrid
                );

            roomConstructor.CreateHallways(
                                   roomPosition,
                                   room.OuterWalls,
                                   roomInstance.transform);

            roomInstances.Add(new(roomInstance, room.Type, room.OuterWalls));
        }
        allRooms = roomInstances;
        transform.position -= (Vector3Int)RoomInfo.Size * RoomGrid.FLOOR_SIZE / 2;
    }

    public void GenerateRoomsContent()
    {
        foreach (GeneratingRoomDescription roomData in allRooms)
        {
            RoomManager roomManager = roomData.RoomInstance.GetComponent<RoomManager>();
            roomManager.Initialize(spawnableEnemies,
                floorObjectsList, new(floorObjectsList, roomData.OuterWalls));
            roomManager.CreateContent(roomData.RoomType);
        }
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
        if (groupedRoomsByWalls.TryGetValue(roomWalls, out IList<RoomInfo> possibleRooms))
        {
            roomInfo = possibleRooms[Random.Range(0, possibleRooms.Count)];
            return true;
        }
        return false;
    }
}