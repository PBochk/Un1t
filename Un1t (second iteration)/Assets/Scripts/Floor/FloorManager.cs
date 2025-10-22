using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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

    private readonly static Range roomsCountRange = new(3, 5);
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

        int roomCount = UnityEngine.Random.Range(roomsCountRange.Start.Value, roomsCountRange.End.Value);
        GenerateFloor(roomCount);
    }

    /// <summary>
    /// Creates all rooms for this level
    /// Each room gets it's own content
    /// </summary>
    private void GenerateFloor(int roomsCount)
    {
        CreateFirstRoom(out FloorGridPosition firstRoomPosition);

        CreateAnotherOneRoom(roomsCount, firstRoomPosition);
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
        List<NeighborRoomDescription> availableRoomsToGenerate = new();
        List<FloorGridPosition> usedPositionsToGenerate = new();

        FloorGridPosition topPosition = FloorGridPosition.Top + roomPosition;
        NeighborRoomDescription top = new(topPosition, GetNeighborWallConfig(topPosition, walls => walls.Bottom));

        FloorGridPosition bottomPosition = FloorGridPosition.Bottom + roomPosition;
        NeighborRoomDescription bottom = new(bottomPosition, GetNeighborWallConfig(bottomPosition, walls => walls.Top));

        FloorGridPosition leftPosition = FloorGridPosition.Left + roomPosition;
        NeighborRoomDescription left = new(leftPosition, GetNeighborWallConfig(leftPosition, walls => walls.Right));

        FloorGridPosition rightPosition = FloorGridPosition.Right + roomPosition;
        NeighborRoomDescription right = new(rightPosition, GetNeighborWallConfig(rightPosition, walls => walls.Left));

        foreach (NeighborRoomDescription room in new NeighborRoomDescription[] { top, bottom, left, right })
            if (!room.Wall.HasValue)
                availableRoomsToGenerate.Add(room);

        int exitsCount = roomsCount >= 0 
            ? UnityEngine.Random.Range(1, Unity.Mathematics.math.min(availableRoomsToGenerate.Count, roomsCount))
            : 0;

        roomsCount -= exitsCount;

        for (var i = 0; i < exitsCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableRoomsToGenerate.Count);

            usedPositionsToGenerate.Add(availableRoomsToGenerate[randomIndex].Position);
            availableRoomsToGenerate[randomIndex].Wall = RoomOuterWalls.Wall.CentreExit;
            availableRoomsToGenerate.RemoveAt(randomIndex);
        }

        
        top.Wall ??= RoomOuterWalls.Wall.Solid;
        bottom.Wall ??= RoomOuterWalls.Wall.Solid;
        left.Wall ??= RoomOuterWalls.Wall.Solid;
        right.Wall ??= RoomOuterWalls.Wall.Solid;

        RoomOuterWalls outerWalls = new(top.Wall.Value, bottom.Wall.Value, left.Wall.Value, right.Wall.Value);


        if (TryChooseTemplateRoom(outerWalls, out RoomInfo roomInfo))
            GenerateRoom(roomInfo, roomPosition);
        else
            ConstructRoom(outerWalls, roomPosition);

        if (usedPositionsToGenerate.Count == 0)
        {
            return;
        }

        int roomsPerBranch = roomsCount / usedPositionsToGenerate.Count;
        int extraRooms = roomsCount % usedPositionsToGenerate.Count;

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
    /// Processes a direction to check for neighboring room and collect available positions
    /// </summary>
    /// <param name="position">Current room position</param>
    /// <param name="getWallFromRoom">Function to extract wall from room's outer walls</param>
    /// <param name="availableRoomPosition">Output position if no room exists</param>
    /// <returns>Wall from neighboring room or null if no room exists</returns>
    /// <summary>
    /// Processes a direction to check for neighboring room and collect available positions
    /// </summary>
    /// <param name="position">Current room position</param>
    /// <param name="getWallFromRoom">Function to extract wall from room's outer walls</param>
    /// <param name="availableRoomPosition">Output position if no room exists</param>
    /// <returns>Wall from neighboring room or null if no room exists</returns>
    private RoomOuterWalls.Wall? GetNeighborWallConfig(in FloorGridPosition position,
        Func<RoomOuterWalls, RoomOuterWalls.Wall> getWallFromRoom)
    {
        if (rooms[position] is RoomInfo room)
        {
            RoomOuterWalls.Wall neighborWall = getWallFromRoom(room.OuterWalls);
            RoomOuterWalls.Wall result = new(
                new RoomOuterWalls.Wall.WallPart(neighborWall.First.IsEmpty),
                new RoomOuterWalls.Wall.WallPart(neighborWall.Middle.IsEmpty),
                new RoomOuterWalls.Wall.WallPart(neighborWall.Last.IsEmpty)
            );
            return result;
        }
        return null;
    }

    #region RoomGrid
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
            get => this[position.X, position.Y];
            set => this[position.X, position.Y] = value;
        }
    }
    #endregion

    //Replace to anonym type
    #region RoomDescription
    /// <summary>
    /// Used only in CreateAnotherOneRoom() method to chain outer wall and it's neighbor FloorGridPosition
    /// </summary>
    public class NeighborRoomDescription
    {
        public FloorGridPosition Position { get; set; }
        public RoomOuterWalls.Wall? Wall { get; set; }

        public NeighborRoomDescription(FloorGridPosition position, RoomOuterWalls.Wall? wall)
        {
            Position = position;
            Wall = wall;
        }
    }
    #endregion

    //TODO: next methods should be refactored and moved to a separate class

    public void ConstructRoom(in RoomOuterWalls roomOuterWalls, in FloorGridPosition position)
    {
        GameObject roomInstance = Instantiate(roomTemplate, (Vector2)((Vector2Int)position * RoomInfo.SIZE), Quaternion.identity, transform);

        CreateWall(roomInstance.transform, roomOuterWalls.Top, baseWallPart,
            new Vector2(0 - 6, 10 - 5), WallDirection.Horizontal);
        CreateWall(roomInstance.transform, roomOuterWalls.Bottom, baseWallPart,
            new Vector2(0 - 6, 0 - 5), WallDirection.Horizontal);
        CreateWall(roomInstance.transform, roomOuterWalls.Left, sideWallPart,
            new Vector2(-3f + 0.333f - 6, 2 - 5), WallDirection.Vertical);
        CreateWall(roomInstance.transform, roomOuterWalls.Right, sideWallPart,
            new Vector2(16 - 0.666f - 6, 2 - 5), WallDirection.Vertical);

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