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

    [SerializeField] private GameObject standardRoomGround;

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

        RoomInfo firstRoom = availableStartRooms[UnityEngine.Random.Range(0, availableStartRooms.Length)].Info;

        GenerateRoom(firstRoom, firstRoomPosition);

        firstRoomPosition += GetOppositeExitDirection(firstRoom.OuterWalls);
    }

    /// <summary>
    /// Define which one direction of room is free
    /// </summary>
    private FloorGridPosition GetOppositeExitDirection(RoomOuterWalls outerWalls)
    {
        static bool HasExit(RoomOuterWalls.Wall wall) => wall.First.IsEmpty || wall.Middle.IsEmpty || wall.Last.IsEmpty;

        if (HasExit(outerWalls.Top)) return FloorGridPosition.Top;
        if (HasExit(outerWalls.Bottom)) return FloorGridPosition.Bottom;
        if (HasExit(outerWalls.Left)) return FloorGridPosition.Left;
        if (HasExit(outerWalls.Right)) return FloorGridPosition.Right;

        throw new Exception("Start room hasn't got any exits");
    }


    /// <summary>
    /// Creates additional rooms recursively based on available positions
    /// </summary>
    /// <param name="roomsCount">Number of rooms to create</param>
    /// <param name="roomPosition">Position where to create the room</param>
    private void CreateAnotherOneRoom(int roomsCount, in FloorGridPosition roomPosition)
    {
        if (rooms[roomPosition] != null) return;

        List<NeighborRoomDescription> availableRoomsToGenerate = new();
        List<FloorGridPosition> usedPositionsToGenerate = new();

        FloorGridPosition topPosition = FloorGridPosition.Top + roomPosition;
        NeighborRoomDescription topRoomDescription = new(topPosition, GetNeighborWallConfig(topPosition, walls => walls.Bottom));

        FloorGridPosition bottomPosition = FloorGridPosition.Bottom + roomPosition;
        NeighborRoomDescription bottomRoomDescription = new(bottomPosition, GetNeighborWallConfig(bottomPosition, walls => walls.Top));

        FloorGridPosition leftPosition = FloorGridPosition.Left + roomPosition;
        NeighborRoomDescription leftRoomDescription = new(leftPosition, GetNeighborWallConfig(leftPosition, walls => walls.Right));

        FloorGridPosition rightPosition = FloorGridPosition.Right + roomPosition;
        NeighborRoomDescription rightRoomDescription = new(rightPosition, GetNeighborWallConfig(rightPosition, walls => walls.Left));

        foreach (NeighborRoomDescription room in new NeighborRoomDescription[] { topRoomDescription, bottomRoomDescription, leftRoomDescription, rightRoomDescription })
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

        topRoomDescription.Wall ??= RoomOuterWalls.Wall.Solid;
        bottomRoomDescription.Wall ??= RoomOuterWalls.Wall.Solid;
        leftRoomDescription.Wall ??= RoomOuterWalls.Wall.Solid;
        rightRoomDescription.Wall ??= RoomOuterWalls.Wall.Solid;

        RoomOuterWalls outerWalls = new(topRoomDescription.Wall.Value, bottomRoomDescription.Wall.Value, leftRoomDescription.Wall.Value, rightRoomDescription.Wall.Value);


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
        GameObject roomInstance = Instantiate(room.RoomPrefab, (Vector2)((Vector2Int)position * RoomInfo.Size),
            Quaternion.identity, transform);

        rooms[position] = room;

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

    private void CreateRoomContent(GameObject room)
    {
        RoomManager roomManager = room.GetComponent<RoomManager>();
        roomManager.SetContent(spawnableEnemies.Enemies, enemySpawner, rock);
        roomManager.CreateContent();
    }


    //TODO: next method should be refactored and moved to a separate class,
    //Caching room types.

    private void ConstructRoom(in RoomOuterWalls roomOuterWalls, in FloorGridPosition position)
    {
        GameObject roomInstance = Instantiate(roomTemplate, (Vector2)((Vector2Int)position * RoomInfo.Size), Quaternion.identity, transform);
        Vector2 roomCenter = (Vector2)roomInstance.transform.position;

        Instantiate(topOuterWall, new Vector2(0, 6) + roomCenter, Quaternion.identity, roomInstance.transform)
            .GetComponent<StandardOuterWallBuilder>()
            .SetPartsEmptiness(roomOuterWalls.Top);

        Instantiate(bottomOuterWall, new Vector2(0, -5) + roomCenter, Quaternion.identity, roomInstance.transform)
            .GetComponent<StandardOuterWallBuilder>()
            .SetPartsEmptiness(roomOuterWalls.Bottom);

        Instantiate(leftOuterWall, new Vector2(-8.5f, 0) + roomCenter, Quaternion.identity, roomInstance.transform)
            .GetComponent<StandardOuterWallBuilder>()
            .SetPartsEmptiness(roomOuterWalls.Left);

        Instantiate(rightOuterWall, new Vector2(8.5f, 0) + roomCenter, Quaternion.identity, roomInstance.transform)
            .GetComponent<StandardOuterWallBuilder>()
            .SetPartsEmptiness(roomOuterWalls.Right);

        Instantiate(rightBottomCorner, new Vector2(-8.5f, 5) + roomCenter, Quaternion.identity, roomInstance.transform);
        Instantiate(leftBottomCorner, new Vector2(8.5f, 5) + roomCenter, Quaternion.identity, roomInstance.transform);
        Instantiate(rightTopCorner, new Vector2(-8.5f, -5) + roomCenter, Quaternion.identity, roomInstance.transform);
        Instantiate(leftTopCorner, new Vector2(8.5f, -5) + roomCenter, Quaternion.identity, roomInstance.transform);

        Instantiate(standardRoomGround, roomCenter, Quaternion.identity, roomInstance.transform);

        rooms[position] = new(roomInstance, roomOuterWalls);

        CreateRoomContent(roomInstance);
    }


    private enum WallDirection
    {
        Horizontal, Vertical
    }
}