using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Creates and manages all rooms in the game level
/// </summary>
//TODO: class should be divided according to SRP
public class FloorManager : MonoBehaviour
{
    [SerializeField] private TemplateRoomInfo[] availableCommonRooms;
    [SerializeField] private TemplateRoomInfo[] availableStartRooms;
    [SerializeField] private TemplateRoomInfo errorRoom; //For debug purpose only

    //TODO: next serilize fields should be moved to a separate class
    [SerializeField] private GameObject roomTemplate;
    [SerializeField] private GameObject sideWallPart;
    [SerializeField] private GameObject baseWallPart;
    [SerializeField] private GameObject angleWallPart;

    private readonly static Range roomsCountRange = new(5, 7);
    private readonly RoomGrid rooms = new();
    private Dictionary<RoomOuterWalls, ImmutableList<RoomInfo>> groupedRoomsByWalls;

    void Awake()
    {
        groupedRoomsByWalls = availableCommonRooms
            .GroupBy(room => room.Info.OuterWalls)
            .ToDictionary(
                group => group.Key,
                group => group.Select(template => template.Info).ToImmutableList()
        );

        GenerateFloor(UnityEngine.Random.Range(roomsCountRange.Start.Value,
            roomsCountRange.End.Value));
    }

    /// <summary>
    /// Creates all rooms for this level
    /// Each room gets it's own content
    /// </summary>
    private void GenerateFloor(int roomsCount)
    {
        GenerateRoom(RoomInfo.ConstructRoom(new RoomOuterWalls(new(), new(), new(), new()), roomTemplate, sideWallPart, baseWallPart), new(0, 15));
        //CreateFirstRoom(out FloorGridPosition firstRoomPosition);

        //CreateAnotherOneRoom(--roomsCount, firstRoomPosition);

    }

    /// <summary>
    /// Creates the first room in the center of the floor
    /// </summary>
    /// <param name="firstRoomPosition">Position of the first room</param>
    private void CreateFirstRoom(out FloorGridPosition firstRoomPosition)
    {
        firstRoomPosition = new FloorGridPosition(RoomGrid.FLOOR_SIZE / 2, RoomGrid.FLOOR_SIZE / 2);
        GenerateRoom(availableStartRooms[1].Info, firstRoomPosition);
        firstRoomPosition += new FloorGridPosition(-1, 0);
    }

    /// <summary>
    /// Creates additional rooms recursively based on available positions
    /// </summary>
    /// <param name="roomsCount">Number of rooms to create</param>
    /// <param name="roomPosition">Position where to create the room</param>
    private void CreateAnotherOneRoom(int roomsCount, in FloorGridPosition roomPosition)
    {
        if (roomsCount <= 0) return;

        List<(FloorGridPosition roomPosition, RoomOuterWalls.Wall? wall)> availableRoomsToGenerate = new();
        List<FloorGridPosition> usedPositionsToGenerate = new();

        ProcessDirection(roomPosition, FloorGridPosition.Top, walls => walls.Bottom, availableRoomsToGenerate,
            out RoomOuterWalls.Wall? topWall);
        ProcessDirection(roomPosition, FloorGridPosition.Bottom, walls => walls.Top, availableRoomsToGenerate,
            out RoomOuterWalls.Wall? bottomWall);
        ProcessDirection(roomPosition, FloorGridPosition.Left, walls => walls.Right, availableRoomsToGenerate,
            out RoomOuterWalls.Wall? leftWall);
        ProcessDirection(roomPosition, FloorGridPosition.Right, walls => walls.Left, availableRoomsToGenerate,
            out RoomOuterWalls.Wall? rightWall);

        int exitsCount = UnityEngine.Random.Range(1, availableRoomsToGenerate.Count + 1);
        roomsCount -= exitsCount;

        for (var i = 0; i < exitsCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableRoomsToGenerate.Count);

            (FloorGridPosition roomPosition, RoomOuterWalls.Wall? wall) emptyRoom = availableRoomsToGenerate[randomIndex];
            usedPositionsToGenerate.Add(availableRoomsToGenerate[randomIndex].roomPosition);
            availableRoomsToGenerate.RemoveAt(randomIndex);

            emptyRoom.wall = RoomOuterWalls.Wall.CentreExit;
        }

        topWall ??= RoomOuterWalls.Wall.Empty;
        bottomWall ??= RoomOuterWalls.Wall.Empty;
        leftWall ??= RoomOuterWalls.Wall.Empty;
        rightWall ??= RoomOuterWalls.Wall.Empty;

        RoomOuterWalls outerWalls = new(topWall.Value, bottomWall.Value, leftWall.Value, rightWall.Value);

        GenerateRoom(ChooseRoom(outerWalls), roomPosition);

        if (availableRoomsToGenerate.Count == 0) return;

        int roomsPerBranch = roomsCount / availableRoomsToGenerate.Count;
        int extraRooms = roomsCount % availableRoomsToGenerate.Count;

        int index = 0;
        foreach (FloorGridPosition position in usedPositionsToGenerate)
        {
            int roomsForThisBranch = roomsPerBranch;
            if (index < extraRooms) roomsForThisBranch++;

            CreateAnotherOneRoom(roomsForThisBranch, position);
            index++;
        }
    }

    /// <summary>
    /// Instantiates a room prefab at the specified position
    /// </summary>
    /// <param name="room">Room to instantiate</param>
    /// <param name="floorGridPosition">Position to place the room</param>
    private void GenerateRoom(RoomInfo room, in FloorGridPosition floorGridPosition)
    {
        rooms[floorGridPosition] = room;
        Instantiate(room.RoomPrefab, (Vector2)((Vector2Int)floorGridPosition * RoomInfo.SIZE),
            Quaternion.identity, transform);
    }

    /// <summary>
    /// Chooses a room based on the specified outer walls configuration
    /// </summary>
    /// <param name="roomWalls">Outer walls configuration to match</param>
    /// <returns>Matching room or error room if none found</returns>
    private RoomInfo ChooseRoom(in RoomOuterWalls roomWalls)
    {
        if (groupedRoomsByWalls.TryGetValue(roomWalls, out ImmutableList<RoomInfo> possibleRooms)
            && possibleRooms.Count > 0)
        {
            return possibleRooms[UnityEngine.Random.Range(0, possibleRooms.Count)];
        }
        return RoomInfo.ConstructRoom(roomWalls, roomTemplate, sideWallPart, baseWallPart);
    }

    /// <summary>
    /// Gets the wall from a neighboring room in the specified direction
    /// </summary>
    /// <param name="position">Current room position</param>
    /// <param name="direction">Direction to check</param>
    /// <param name="getWallFromRoom">Function to extract wall from room's outer walls</param>
    /// <param name="availableRoomPosition">Output position if no room exists</param>
    /// <returns>Wall from neighboring room or null if no room exists</returns>
    private RoomOuterWalls.Wall? GetNeighborWall(in FloorGridPosition position,
        in FloorGridPosition direction,
    Func<RoomOuterWalls, RoomOuterWalls.Wall> getWallFromRoom,
    out FloorGridPosition availableRoomPosition)
    {
        availableRoomPosition = position + direction;
        if (rooms[availableRoomPosition] is RoomInfo room)
        {
            RoomOuterWalls.Wall neighborWall = getWallFromRoom(room.OuterWalls);
            return new RoomOuterWalls.Wall(
                new RoomOuterWalls.Wall.WallPart(neighborWall.First.IsEmpty),
                new RoomOuterWalls.Wall.WallPart(neighborWall.Middle.IsEmpty),
                new RoomOuterWalls.Wall.WallPart(neighborWall.Last.IsEmpty)
            );
        }
        return null;
    }

    /// <summary>
    /// Processes a direction to check for neighboring room and collect available positions
    /// </summary>
    /// <param name="roomPosition">Current room position</param>
    /// <param name="direction">Direction to process</param>
    /// <param name="getWallFromRoom">Function to extract wall from room's outer walls</param>
    /// <param name="availableRoomsToGenerate">List to add available positions to</param>
    /// <param name="wall">Output wall from neighboring room</param>
    private void ProcessDirection(in FloorGridPosition roomPosition, in FloorGridPosition direction,
    Func<RoomOuterWalls, RoomOuterWalls.Wall> getWallFromRoom,
    List<(FloorGridPosition position, RoomOuterWalls.Wall? wall)> availableRoomsToGenerate,
    out RoomOuterWalls.Wall? wall)
    {
        wall = GetNeighborWall(roomPosition, direction, getWallFromRoom, out FloorGridPosition availableRoomPosition);
        if (!wall.HasValue)
            availableRoomsToGenerate.Add(new(availableRoomPosition, wall));
    }

    /// <summary>
    /// Draws the current floor map in the console
    /// </summary>
    /// <param name="stepDescription">Description of the current generation step</param>
    /// <summary>
    /// Manages the grid of rooms in the floor
    /// </summary>
    private class RoomGrid
    {
        public const int FLOOR_SIZE = 16;

        private readonly RoomInfo[,] rooms;

        public RoomGrid()
        {
            rooms = new RoomInfo[FLOOR_SIZE, FLOOR_SIZE];
        }

        /// <summary>
        /// Gets or sets a room at the specified coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public RoomInfo this[int x, int y]
        {
            get => rooms[x, y];
            set
            {
                if (x >= FLOOR_SIZE || y >= FLOOR_SIZE)
                {
                    Debug.LogError($"Point ({x}, {y}) is out of RoomGrid");
                    return;
                }
                rooms[x, y] = value;
            }
        }

        /// <summary>
        /// Gets or sets a room at the specified grid position
        /// </summary>
        /// <param name="position">Grid position</param>
        public RoomInfo this[in FloorGridPosition position]
        {
            get => rooms[position.X, position.Y];
            set => rooms[position.X, position.Y] = value;
        }
    }
}