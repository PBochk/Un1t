using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UnityEngine;

/// <summary>
/// Creates and manages all rooms in the game level
/// </summary>
public class FloorManager : MonoBehaviour
{
    [SerializeField] private RoomInfo[] availableRooms;

    private const sbyte FLOOR_SIZE = 16;

    private readonly RoomInfo[,] rooms = new RoomInfo[FLOOR_SIZE, FLOOR_SIZE];

    private Dictionary<RoomExits, ImmutableList<RoomInfo>> groupedRooms;

    void Awake()
    {
        groupedRooms = availableRooms
            .GroupBy(room => room.Exits)
            .ToDictionary(g => g.Key, g => g.ToImmutableList());
        GenerateFloor();
    }

    /// <summary>
    /// Creates all rooms for this level
    /// Each room gets it's own content
    /// </summary>
    private void GenerateFloor()
    {
        CreateRoom(ChooseRoom(new RoomExits(true, true, true, true)), new Vector2Int(FLOOR_SIZE / 2, FLOOR_SIZE / 2));
    }

    private void CreateRoom(RoomInfo room, Vector2Int floorGridPosition)
    {
        rooms[floorGridPosition.x, floorGridPosition.y] = room;
        Instantiate(room.RoomPrefab, (Vector2)(floorGridPosition * RoomInfo.SIZE), Quaternion.identity, transform);
    }

    private RoomInfo ChooseRoom(RoomExits roomExits)
    {
        if (groupedRooms.TryGetValue(roomExits, out var possibleRooms) && possibleRooms.Count > 0)
        {
            return possibleRooms[Random.Range(0, possibleRooms.Count)];
        }

        throw new System.Exception($"No rooms found for exits: " +
            $"Top wall has an exit - {roomExits.HasTopWallExit} " +
            $"Bottom wall has an exit - {roomExits.HasBottomWallExit} " +
            $"Left wall has an exit - {roomExits.HasLeftWallExit} " +
            $"Right wall has an exit - {roomExits.HasRightWallExit} ");
    }
}