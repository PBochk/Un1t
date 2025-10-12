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
        CreateFirstRoom(out FloorGridPosition firstRoomPosition);

        CreateAnotherOneRoom(--roomsCount, firstRoomPosition);

    }

    /// <summary>
    /// Creates the first room in the center of the floor
    /// </summary>
    /// <param name="firstRoomPosition">Position of the first room</param>
    private void CreateFirstRoom(out FloorGridPosition firstRoomPosition)
    {
        firstRoomPosition = new FloorGridPosition(RoomGrid.FLOOR_SIZE / 2, RoomGrid.FLOOR_SIZE / 2);
        GenerateRoom(availableStartRooms[1].Info, firstRoomPosition); //availableStartRooms[UnityEngine.Random.Range(0, availableStartRooms.Length)].Info
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

            //TODO: refactor it
            if (emptyRoom.roomPosition == roomPosition + FloorGridPosition.Top)
                topWall = RoomOuterWalls.Wall.CentreExit;
            else if (emptyRoom.roomPosition == roomPosition + FloorGridPosition.Bottom)
                bottomWall = RoomOuterWalls.Wall.CentreExit;
            else if (emptyRoom.roomPosition == roomPosition + FloorGridPosition.Left)
                leftWall = RoomOuterWalls.Wall.CentreExit;
            else if (emptyRoom.roomPosition == roomPosition + FloorGridPosition.Right)
                rightWall = RoomOuterWalls.Wall.CentreExit;
        }
        Debug.Log($"{topWall.HasValue} {bottomWall.HasValue} {leftWall.HasValue} {rightWall.HasValue}");
        topWall ??= RoomOuterWalls.Wall.Solid;
        bottomWall ??= RoomOuterWalls.Wall.Solid;
        leftWall ??= RoomOuterWalls.Wall.Solid;
        rightWall ??= RoomOuterWalls.Wall.Solid;

        RoomOuterWalls outerWalls = new(topWall.Value, bottomWall.Value, leftWall.Value, rightWall.Value);

        if (TryChooseTemplateRoom(outerWalls, out RoomInfo roomInfo))
            GenerateRoom(roomInfo, roomPosition);
        else
            ConstructRoom(outerWalls, roomPosition);

        Debug.Log($"Room walls configuration at position ({roomPosition.X}, {roomPosition.Y}): " +
          $"Top: First={outerWalls.Top.First.IsEmpty}, Middle={outerWalls.Top.Middle.IsEmpty}, Last={outerWalls.Top.Last.IsEmpty}; " +
          $"Bottom: First={outerWalls.Bottom.First.IsEmpty}, Middle={outerWalls.Bottom.Middle.IsEmpty}, Last={outerWalls.Bottom.Last.IsEmpty}; " +
          $"Left: First={outerWalls.Left.First.IsEmpty}, Middle={outerWalls.Left.Middle.IsEmpty}, Last={outerWalls.Left.Last.IsEmpty}; " +
          $"Right: First={outerWalls.Right.First.IsEmpty}, Middle={outerWalls.Right.Middle.IsEmpty}, Last={outerWalls.Right.Last.IsEmpty}");

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
    /// <param name="position">Position to place the room</param>
    private void GenerateRoom(RoomInfo room, in FloorGridPosition position)
    {
        rooms[position] = room;
        Instantiate(room.RoomPrefab, (Vector2)((Vector2Int)position * RoomInfo.SIZE),
            Quaternion.identity, transform);
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
    #region RoomGrid
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
    #endregion

    //TODO: next methods should be refactored and moved to a separate class

    public void ConstructRoom(in RoomOuterWalls roomOuterWalls, in FloorGridPosition position)
    {
        GameObject roomInstance = Instantiate(roomTemplate, (Vector2)((Vector2Int)position * RoomInfo.SIZE), Quaternion.identity, transform);

        CreateWall(roomInstance.transform, roomOuterWalls.Top, baseWallPart, new Vector2(0 - 6, 10 - 5), WallDirection.Horizontal);
        CreateWall(roomInstance.transform, roomOuterWalls.Bottom, baseWallPart, new Vector2(0 - 6, 0 - 5), WallDirection.Horizontal);
        CreateWall(roomInstance.transform, roomOuterWalls.Left, sideWallPart, new Vector2(-3f + 0.333f - 6, 2 - 5), WallDirection.Vertical);
        CreateWall(roomInstance.transform, roomOuterWalls.Right, sideWallPart, new Vector2(16 - 0.666f - 6, 2 - 5), WallDirection.Vertical);

        rooms[position] = new RoomInfo(roomInstance, roomOuterWalls);
    }

    private static void CreateWall(Transform parent, in RoomOuterWalls.Wall wall,
        GameObject wallPartObject, in Vector2 startPosition, WallDirection direction)
    {
        Vector3 currentPosition = startPosition;
        Vector3 step = direction == WallDirection.Horizontal ? new Vector2(6.333f, 0f) : new Vector2(0, 3);

        foreach (RoomOuterWalls.Wall.WallPart wallPart in
            new RoomOuterWalls.Wall.WallPart[] { wall.First, wall.Middle, wall.Last })
        {
            if (!wallPart.IsEmpty)
                Instantiate(wallPartObject, currentPosition + parent.position, Quaternion.identity, parent);
            
            currentPosition += step;
        }
    }

    private enum WallDirection
    {
        Horizontal, Vertical
    }
}