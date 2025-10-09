using System;
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


    private const int FLOOR_SIZE = 16;

    private readonly static Range roomsCountRange = new(5, 7);

    private readonly RoomInfo[,] rooms = new RoomInfo[FLOOR_SIZE, FLOOR_SIZE];

    private Dictionary<RoomExits, ImmutableList<RoomInfo>> groupedRooms;

    void Awake()
    {
        groupedRooms = availableRooms
            .GroupBy(room => room.Exits)
            .ToDictionary(group => group.Key, group => group.ToImmutableList());
        GenerateFloor(UnityEngine.Random.Range(roomsCountRange.Start.Value, 
            roomsCountRange.End.Value));
    }

    /// <summary>
    /// Creates all rooms for this level
    /// Each room gets it's own content
    /// </summary>
    private void GenerateFloor(int roomsCount)
    {
        CreateFirstRoom(out FloorGridPosition firstRoomPosition);
        CreateAnotherOneRoom(--roomsCount, firstRoomPosition);
    }

    private void CreateFirstRoom(out FloorGridPosition firstRoomPosition)
    {
        firstRoomPosition = new FloorGridPosition(FLOOR_SIZE / 2, FLOOR_SIZE / 2);

        GenerateRoom(ChooseRoom(new RoomExits(false, false, true, false)),
            firstRoomPosition);
    }

    private void CreateAnotherOneRoom(int roomsCount, FloorGridPosition floorGridPosition)
    {
        if (roomsCount <= 0) return;

        List<FloorGridPosition> availableRoomPositions = new();
        List<FloorGridPosition> usedRoomPositions = new();

        foreach (FloorGridPosition direction in FloorGridPosition.Directions)
        {
            FloorGridPosition newRoomPosition = direction + floorGridPosition;
            if (rooms[newRoomPosition.Y, newRoomPosition.X] == null)
                    availableRoomPositions.Add(new(newRoomPosition.Y, newRoomPosition.X));
        }

        int exitsCount = UnityEngine.Random.Range(0, availableRoomPositions.Count + 1);
        roomsCount -= exitsCount;

        for (int i = 0; i < exitsCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableRoomPositions.Count);

            FloorGridPosition exit = availableRoomPositions[randomIndex];
            availableRoomPositions.RemoveAt(randomIndex);
            usedRoomPositions.Add(exit);
        }

        GenerateRoom(ChooseRoom(new RoomExits(floorGridPosition, availableRoomPositions.ToArray())),
            floorGridPosition);

        foreach (FloorGridPosition position in availableRoomPositions)
        {
            CreateAnotherOneRoom(roomsCount, position);
        }

    }

    private void GenerateRoom(RoomInfo room, FloorGridPosition floorGridPosition)
    {
        rooms[floorGridPosition.X, floorGridPosition.Y] = room;
        Instantiate(room.RoomPrefab, (Vector2)((Vector2Int)floorGridPosition * RoomInfo.SIZE), 
            Quaternion.identity, transform);
    }

    private RoomInfo ChooseRoom(RoomExits roomExits)
    {
        if (groupedRooms.TryGetValue(roomExits, out ImmutableList<RoomInfo> possibleRooms) 
            && possibleRooms.Count > 0)
        {
            return possibleRooms[UnityEngine.Random.Range(0, possibleRooms.Count)];
        }
        return availableRooms[0];

        throw new Exception($"No rooms found for exits: " +
            $"; Top wall has an exit - {roomExits.HasTopWallExit} " +
            $"; Bottom wall has an exit - {roomExits.HasBottomWallExit} " +
            $"; Left wall has an exit - {roomExits.HasLeftWallExit} " +
            $"; Right wall has an exit - {roomExits.HasRightWallExit} ");
    }
}