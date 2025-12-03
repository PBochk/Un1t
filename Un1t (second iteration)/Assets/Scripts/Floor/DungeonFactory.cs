using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

public class DungeonFactory
{
    private Random random;

    /// <summary>
    /// Offset for the first room position. All other rooms are positioned relative to this.
    /// </summary>
    private static readonly FloorGridPosition firstRoomOffset = new(RoomGrid.FLOOR_SIZE/2, RoomGrid.FLOOR_SIZE / 2);

    /// <summary>
    /// Create and generate a complete dungeon.
    /// </summary>
    /// <param name="floorNumber">Current floor number (affects total room count)</param>
    /// <param name="minDistance">Minimum distance from entrance to exit</param>
    /// <param name="maxDistance">Maximum distance from entrance to exit</param>
    /// <param name="bonusProbability">Probability of a room being a bonus room</param>
    /// <param name="seed">Random seed for reproducible generation (null = random)</param>
    /// <param name="entropy">Controls randomness (0.0-1.0):
    /// - 0.0: Minimal extra connections, straight paths
    /// - 0.5: Balanced variety
    /// - 1.0: Maximum extra connections and variety</param>
    /// <param name="roomMultiplier">Base multiplier for room count formula</param>
    /// <param name="roomFloorCoef">Floor number coefficient for room count formula</param>
    /// <returns>A list of fully generated Room instances</returns>
    public List<Room> CreateDungeon(
        int floorNumber = 0,
        int minDistance = 50,
        int maxDistance = 70,
        double bonusProbability = 0.15,
        int? seed = null,
        double entropy = 0.2,
        int roomMultiplier = 1,
        int roomFloorCoef = 1)
    {
        if (seed.HasValue)
        {
            random = new Random(seed.Value);
        }
        else
        {
            random = new Random();
        }

        entropy = Math.Max(0.0, Math.Min(1.0, entropy));

        Dungeon dungeon = new()
        {
            FloorNumber = floorNumber,
            PathLength = random.Next(minDistance, maxDistance + 1)
        };

        int totalRooms = dungeon.PathLength * (roomMultiplier + floorNumber * roomFloorCoef);

        dungeon.GridSize = (int)(Math.Sqrt(totalRooms) * 1.5) + 2;
        if (dungeon.GridSize < dungeon.PathLength + 2)
        {
            dungeon.GridSize = dungeon.PathLength + 2;
        }

        int center = dungeon.GridSize / 2;
        dungeon.EntrancePosition = new FloorGridPosition(center, center);

        PlaceRooms(dungeon, totalRooms);

        ConnectRooms(dungeon, entropy);

        AddBonusRooms(dungeon, bonusProbability);

        List<Room> allRooms = new();

        foreach (KeyValuePair<FloorGridPosition, Room> roomWithPosition in dungeon.grid)
        {
            FloorGridPosition gridPos = roomWithPosition.Key;
            Room room = roomWithPosition.Value;

            int worldX = firstRoomOffset.X + (gridPos.X - dungeon.EntrancePosition.X);
            int worldY = firstRoomOffset.Y + (gridPos.Y - dungeon.EntrancePosition.Y);
            FloorGridPosition worldPosition = new (worldX, worldY);

            UpdateOuterWalls(room);

            if (gridPos.Equals(dungeon.EntrancePosition))
            {
                room.Type = Room.RoomType.Entrance;
            }
            else if (gridPos.Equals(dungeon.ExitPosition))
            {
                room.Type = Room.RoomType.Exit;
            }

            room.GridPosition = worldPosition;
            allRooms.Add(room);
        }

        return allRooms;
    }

    private void UpdateOuterWalls(Room room)
    {
        RoomOuterWalls.Wall topWall = room.HasSouthPassage ?
            RoomOuterWalls.Wall.CentreExit :
            RoomOuterWalls.Wall.Solid;

        RoomOuterWalls.Wall bottomWall = room.HasNorthPassage ?
            RoomOuterWalls.Wall.CentreExit :
            RoomOuterWalls.Wall.Solid;

        RoomOuterWalls.Wall leftWall = room.HasWestPassage ?
            RoomOuterWalls.Wall.CentreExit :
            RoomOuterWalls.Wall.Solid;

        RoomOuterWalls.Wall rightWall = room.HasEastPassage ?
            RoomOuterWalls.Wall.CentreExit :
            RoomOuterWalls.Wall.Solid;

        room.OuterWalls = new RoomOuterWalls(topWall, bottomWall, leftWall, rightWall);
    }

    private void PlaceRooms(Dungeon dungeon, int totalRooms)
    {
        List<FloorGridPosition> mainPath = CreateGuaranteedPath(dungeon, dungeon.PathLength);

        dungeon.MainPath = mainPath;

        HashSet<FloorGridPosition> placed = new(mainPath);
        foreach (FloorGridPosition pos in mainPath)
        {
            dungeon.grid[pos] = new();
        }

        dungeon.ExitPosition = mainPath[^1];

        HashSet<FloorGridPosition> candidates = new();
        foreach (FloorGridPosition position in placed)
        {
            foreach (FloorGridPosition adjacentPosition in GetAdjacentPositions(dungeon, position))
            {
                if (!placed.Contains(adjacentPosition))
                {
                    candidates.Add(adjacentPosition);
                }
            }
        }

        while (placed.Count < totalRooms && candidates.Count > 0)
        {
            FloorGridPosition position = candidates.ElementAt(random.Next(candidates.Count));
            candidates.Remove(position);

            Room room = new();
            dungeon.grid[position] = room;
            placed.Add(position);

            foreach (FloorGridPosition adjacentPosition in GetAdjacentPositions(dungeon, position))
            {
                if (!placed.Contains(adjacentPosition))
                {
                    candidates.Add(adjacentPosition);
                }
            }
        }
    }

    private void ConnectRooms(Dungeon dungeon, double entropy)
    {
        List<FloorGridPosition> mainPath = dungeon.MainPath;

        if (mainPath != null && mainPath.Count > 0)
        {
            for (var i = 0; i < mainPath.Count - 1; i++)
            {
                AddPassage(dungeon, mainPath[i], mainPath[i + 1]);
            }
        }

        HashSet<FloorGridPosition> mainPathSet = mainPath != null ?
            new HashSet<FloorGridPosition>(mainPath) :
            new HashSet<FloorGridPosition>();

        List<FloorGridPosition> connected = GetConnectedRooms(dungeon, dungeon.EntrancePosition);
        List<FloorGridPosition> unconnected = new(dungeon.grid.Keys.Except(connected));

        while (unconnected.Count > 0)
        {
            var foundConnection = false;

            foreach (FloorGridPosition uncon in unconnected.ToList())
            {
                foreach (FloorGridPosition adj in GetAdjacentPositions(dungeon, uncon))
                {
                    if (connected.Contains(adj))
                    {
                        AddPassage(dungeon, uncon, adj);
                        foundConnection = true;
                        break;
                    }
                }
                if (foundConnection)
                {
                    break;
                }
            }

            if (!foundConnection)
            {
                break;
            }

            connected = GetConnectedRooms(dungeon, dungeon.EntrancePosition);
            unconnected = new List<FloorGridPosition>(dungeon.grid.Keys.Except(connected));
        }

        double roomProbability = entropy * 0.5;
        double passageProbability = entropy * 0.6;

        List<FloorGridPosition> positions = new(dungeon.grid.Keys);
        foreach (FloorGridPosition pos in positions)
        {
            if (random.NextDouble() < roomProbability)
            {
                foreach (FloorGridPosition adj in GetAdjacentPositions(dungeon, pos))
                {
                    if (!dungeon.grid.ContainsKey(adj))
                    {
                        continue;
                    }
                    if (random.NextDouble() >= passageProbability)
                    {
                        continue;
                    }

                    if (mainPathSet.Contains(pos) && mainPathSet.Contains(adj))
                    {
                        int posIdx = mainPath.IndexOf(pos);
                        int adjIdx = mainPath.IndexOf(adj);
                        if (Math.Abs(posIdx - adjIdx) == 1)
                        {
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    AddPassage(dungeon, pos, adj);
                }
            }
        }
    }

    /// <summary>
    /// Add a BIDIRECTIONAL passage between two adjacent rooms.
    /// Only works if BOTH rooms exist and are adjacent.
    /// </summary>
    private void AddPassage(Dungeon dungeon, FloorGridPosition pos1, FloorGridPosition pos2)
    {
        if (!dungeon.grid.ContainsKey(pos1) || !dungeon.grid.ContainsKey(pos2))
        {
            return;
        }

        int dx = pos2.X - pos1.X;
        int dy = pos2.Y - pos1.Y;

        if (Math.Abs(dx) + Math.Abs(dy) != 1)
        {
            return;
        }

        Room room1 = dungeon.grid[pos1];
        Room room2 = dungeon.grid[pos2];

        if (dx == 1)
        {
            room1.HasEastPassage = true;
            room2.HasWestPassage = true;
        }
        else if (dx == -1)
        {
            room1.HasWestPassage = true;
            room2.HasEastPassage = true;
        }
        else if (dy == 1)
        {
            room1.HasSouthPassage = true;
            room2.HasNorthPassage = true;
        }
        else if (dy == -1)
        {
            room1.HasNorthPassage = true;
            room2.HasSouthPassage = true;
        }
    }

    /// <summary>
    /// Get all rooms connected to start via existing passages.
    /// </summary>
    private List<FloorGridPosition> GetConnectedRooms(Dungeon dungeon, FloorGridPosition start)
    {
        if (!dungeon.grid.ContainsKey(start))
        {
            return new List<FloorGridPosition>();
        }

        List<FloorGridPosition> visited = new() { start };
        Queue<FloorGridPosition> queue = new();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            FloorGridPosition pos = queue.Dequeue();
            Room room = dungeon.grid[pos];

            var directions = new[]
            {
                (Pos: new FloorGridPosition(pos.X, pos.Y - 1), HasPassage: room.HasNorthPassage, Opposite: Direction.South),
                (Pos: new FloorGridPosition(pos.X, pos.Y + 1), HasPassage: room.HasSouthPassage, Opposite: Direction.North),
                (Pos: new FloorGridPosition(pos.X + 1, pos.Y), HasPassage: room.HasEastPassage, Opposite: Direction.West),
                (Pos: new FloorGridPosition(pos.X - 1, pos.Y), HasPassage: room.HasWestPassage, Opposite: Direction.East)
            };

            foreach (var (Pos, HasPassage, Opposite) in directions)
            {
                if (HasPassage && dungeon.grid.ContainsKey(Pos))
                {
                    Room neighborRoom = dungeon.grid[Pos];
                    bool oppositePassage = false;
                    switch (Opposite)
                    {
                        case Direction.South: oppositePassage = neighborRoom.HasSouthPassage; break;
                        case Direction.North: oppositePassage = neighborRoom.HasNorthPassage; break;
                        case Direction.West: oppositePassage = neighborRoom.HasWestPassage; break;
                        case Direction.East: oppositePassage = neighborRoom.HasEastPassage; break;
                    }

                    if (oppositePassage)
                    {
                        if (!visited.Contains(Pos))
                        {
                            visited.Add(Pos);
                            queue.Enqueue(Pos);
                        }
                    }
                }
            }
        }

        return visited;
    }

    /// <summary>
    /// Get valid adjacent positions within grid bounds.
    /// </summary>
    private List<FloorGridPosition> GetAdjacentPositions(Dungeon dungeon, FloorGridPosition pos)
    {
        int x = pos.X;
        int y = pos.Y;
        List<FloorGridPosition> adjacent = new List<FloorGridPosition>();

        (int dx, int dy)[] directions = { (0, -1), (1, 0), (0, 1), (-1, 0) };
        foreach ((int dx, int dy) in directions)
        {
            FloorGridPosition newPos = new(x + dx, y + dy);
            if (0 <= newPos.X && newPos.X < dungeon.GridSize &&
                0 <= newPos.Y && newPos.Y < dungeon.GridSize)
            {
                adjacent.Add(newPos);
            }
        }

        return adjacent;
    }

    /// <summary>
    /// Create a guaranteed path of exact length starting from entrance.
    /// Uses a random walk that avoids revisiting positions.
    /// </summary>
    private List<FloorGridPosition> CreateGuaranteedPath(Dungeon dungeon, int length)
    {
        List<FloorGridPosition> path = new() { dungeon.EntrancePosition };
        HashSet<FloorGridPosition> visited = new() { dungeon.EntrancePosition };
        FloorGridPosition current = dungeon.EntrancePosition;

        for (int i = 0; i < length; i++)
        {
            List<FloorGridPosition> adjacent = GetAdjacentPositions(dungeon, current);
            List<FloorGridPosition> unvisited = adjacent.Where(pos => !visited.Contains(pos)).ToList();

            if (unvisited.Count == 0)
            {
                if (path.Count > 1)
                {
                    FloorGridPosition removed = path[^1];
                    path.RemoveAt(path.Count - 1);
                    visited.Remove(removed);
                    current = path[^1];
                    continue;
                }
                else
                {
                    break;
                }
            }

            FloorGridPosition nextPos = unvisited[random.Next(unvisited.Count)];
            path.Add(nextPos);
            visited.Add(nextPos);
            current = nextPos;
        }

        return path;
    }

    /// <summary>
    /// Mark some rooms as bonus rooms.
    /// </summary>
    private void AddBonusRooms(Dungeon dungeon, double bonusProbability)
    {
        List<FloorGridPosition> regularRooms = dungeon.grid.Keys
            .Where(pos => !pos.Equals(dungeon.EntrancePosition) && !pos.Equals(dungeon.ExitPosition))
            .ToList();

        if (regularRooms.Count == 0)
        {
            return;
        }

        int numBonus = Math.Max(1, (int)(regularRooms.Count * bonusProbability));
        List<FloorGridPosition> bonusPositions = regularRooms
            .OrderBy(x => random.Next())
            .Take(Math.Min(numBonus, regularRooms.Count))
            .ToList();

        foreach (FloorGridPosition pos in bonusPositions)
        {
            dungeon.grid[pos].IsBonus = true;
        }
    }

    private enum Direction
    {
        North,
        South,
        East,
        West
    }

    #region Dungeon
    /// <summary>
    /// Internal dungeon data structure for generation process.
    /// </summary>
    private class Dungeon
    {
        public Dictionary<FloorGridPosition, Room> grid = new();
        public int FloorNumber { get; set; }
        public int PathLength { get; set; }
        public int GridSize { get; set; }
        public FloorGridPosition EntrancePosition { get; set; }
        public FloorGridPosition ExitPosition { get; set; }
        public List<FloorGridPosition> MainPath { get; set; }
    }
    #endregion

    #region Room
    /// <summary>
    /// Represents a room in the dungeon with passages and type information.
    /// </summary>
    public class Room
    {
        public RoomOuterWalls OuterWalls { get; set; }
        public bool IsBonus { get; set; }
        public RoomType Type { get; set; }
        public FloorGridPosition GridPosition { get; set; }

        public bool HasNorthPassage { get; set; }
        public bool HasSouthPassage { get; set; }
        public bool HasEastPassage { get; set; }
        public bool HasWestPassage { get; set; }

        public Room()
        {
            HasNorthPassage = false;
            HasSouthPassage = false;
            HasEastPassage = false;
            HasWestPassage = false;
            IsBonus = false;
            Type = RoomType.Regular;
        }

        /// <summary>
        /// Types of rooms in the dungeon.
        /// </summary>
        public enum RoomType
        {
            Regular,
            Entrance,
            Exit,
            Bonus
        }
    }
    #endregion
}