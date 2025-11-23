using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class DungeonFactory
{
    private Random random;

    public List<Room> CreateDungeon(
        int floorNumber = 0,
        int minDistance = 25,
        int maxDistance = 27,
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

        Dungeon dungeon = new Dungeon();
        dungeon.FloorNumber = floorNumber;

        dungeon.PathLength = random.Next(minDistance, maxDistance + 1);

        int totalRooms = dungeon.PathLength * (roomMultiplier + floorNumber * roomFloorCoef);

        dungeon.GridSize = (int)(Math.Sqrt(totalRooms) * 1.5) + 2;
        if (dungeon.GridSize < dungeon.PathLength + 2)
        {
            dungeon.GridSize = dungeon.PathLength + 2;
        }

        int center = dungeon.GridSize / 2;
        dungeon.entrance = new Tuple<int, int>(center, center);

        PlaceRooms(dungeon, totalRooms);

        ConnectRooms(dungeon, entropy);

        AddBonusRooms(dungeon, bonusProbability);

        List<Room> allRooms = new List<Room>();

        foreach (var kvp in dungeon.grid)
        {
            Tuple<int, int> pos = kvp.Key;
            Room room = kvp.Value;

            // Update outer walls based on passage connections
            UpdateOuterWalls(room);

            if (pos.Equals(dungeon.entrance))
            {
                room.Type = Room.RoomType.Entrance;
            }
            else if (pos.Equals(dungeon.exit))
            {
                room.Type = Room.RoomType.Exit;
            }

            room.GridPosition = new FloorGridPosition(pos.Item1, pos.Item2);
            allRooms.Add(room);
        }

        return allRooms;
    }

    private void UpdateOuterWalls(Room room)
    {
        // South = Top, North = Bottom, West = Left, East = Right
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
        List<Tuple<int, int>> mainPath = CreateGuaranteedPath(dungeon, dungeon.PathLength);

        dungeon.mainPath = mainPath;

        HashSet<Tuple<int, int>> placed = new HashSet<Tuple<int, int>>(mainPath);
        foreach (Tuple<int, int> pos in mainPath)
        {
            Room room = new();
            dungeon.grid[pos] = room;
        }

        dungeon.exit = mainPath[mainPath.Count - 1];

        HashSet<Tuple<int, int>> candidates = new HashSet<Tuple<int, int>>();
        foreach (Tuple<int, int> pos in placed)
        {
            foreach (Tuple<int, int> adj in GetAdjacentPositions(dungeon, pos))
            {
                if (!placed.Contains(adj))
                {
                    candidates.Add(adj);
                }
            }
        }

        while (placed.Count < totalRooms && candidates.Count > 0)
        {
            Tuple<int, int> pos = candidates.ElementAt(random.Next(candidates.Count));
            candidates.Remove(pos);

            Room room = new();
            dungeon.grid[pos] = room;
            placed.Add(pos);

            foreach (Tuple<int, int> adj in GetAdjacentPositions(dungeon, pos))
            {
                if (!placed.Contains(adj))
                {
                    candidates.Add(adj);
                }
            }
        }
    }

    private void ConnectRooms(Dungeon dungeon, double entropy)
    {
        List<Tuple<int, int>> mainPath = dungeon.mainPath;

        if (mainPath != null && mainPath.Count > 0)
        {
            for (int i = 0; i < mainPath.Count - 1; i++)
            {
                AddPassage(dungeon, mainPath[i], mainPath[i + 1]);
            }
        }

        HashSet<Tuple<int, int>> mainPathSet = mainPath != null ? new HashSet<Tuple<int, int>>(mainPath) : new HashSet<Tuple<int, int>>();

        List<Tuple<int, int>> connected = GetConnectedRooms(dungeon, dungeon.entrance);
        List<Tuple<int, int>> unconnected = new List<Tuple<int, int>>(dungeon.grid.Keys.Except(connected));

        while (unconnected.Count > 0)
        {
            bool foundConnection = false;

            foreach (Tuple<int, int> uncon in unconnected.ToList())
            {
                foreach (Tuple<int, int> adj in GetAdjacentPositions(dungeon, uncon))
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

            connected = GetConnectedRooms(dungeon, dungeon.entrance);
            unconnected = new List<Tuple<int, int>>(dungeon.grid.Keys.Except(connected));
        }

        double roomProbability = entropy * 0.5;
        double passageProbability = entropy * 0.6;

        List<Tuple<int, int>> positions = new List<Tuple<int, int>>(dungeon.grid.Keys);
        foreach (Tuple<int, int> pos in positions)
        {
            if (random.NextDouble() < roomProbability)
            {
                foreach (Tuple<int, int> adj in GetAdjacentPositions(dungeon, pos))
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

    private void AddPassage(Dungeon dungeon, Tuple<int, int> pos1, Tuple<int, int> pos2)
    {
        if (!dungeon.grid.ContainsKey(pos1) || !dungeon.grid.ContainsKey(pos2))
        {
            return;
        }

        int dx = pos2.Item1 - pos1.Item1;
        int dy = pos2.Item2 - pos1.Item2;

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

    private List<Tuple<int, int>> GetConnectedRooms(Dungeon dungeon, Tuple<int, int> start)
    {
        if (!dungeon.grid.ContainsKey(start))
        {
            return new List<Tuple<int, int>>();
        }

        List<Tuple<int, int>> visited = new List<Tuple<int, int>> { start };
        Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            Tuple<int, int> pos = queue.Dequeue();
            Room room = dungeon.grid[pos];

            var directions = new[]
            {
                (Pos: new Tuple<int, int>(pos.Item1, pos.Item2 - 1), HasPassage: room.HasNorthPassage, Opposite: "south"),
                (Pos: new Tuple<int, int>(pos.Item1, pos.Item2 + 1), HasPassage: room.HasSouthPassage, Opposite: "north"),
                (Pos: new Tuple<int, int>(pos.Item1 + 1, pos.Item2), HasPassage: room.HasEastPassage, Opposite: "west"),
                (Pos: new Tuple<int, int>(pos.Item1 - 1, pos.Item2), HasPassage: room.HasWestPassage, Opposite: "east")
            };

            foreach (var dir in directions)
            {
                if (dir.HasPassage && dungeon.grid.ContainsKey(dir.Pos))
                {
                    Room neighborRoom = dungeon.grid[dir.Pos];
                    bool oppositePassage = false;
                    switch (dir.Opposite)
                    {
                        case "south": oppositePassage = neighborRoom.HasSouthPassage; break;
                        case "north": oppositePassage = neighborRoom.HasNorthPassage; break;
                        case "west": oppositePassage = neighborRoom.HasWestPassage; break;
                        case "east": oppositePassage = neighborRoom.HasEastPassage; break;
                    }

                    if (oppositePassage)
                    {
                        if (!visited.Contains(dir.Pos))
                        {
                            visited.Add(dir.Pos);
                            queue.Enqueue(dir.Pos);
                        }
                    }
                }
            }
        }

        return visited;
    }

    private List<Tuple<int, int>> GetAdjacentPositions(Dungeon dungeon, Tuple<int, int> pos)
    {
        int x = pos.Item1;
        int y = pos.Item2;
        List<Tuple<int, int>> adjacent = new List<Tuple<int, int>>();

        (int dx, int dy)[] directions = { (0, -1), (1, 0), (0, 1), (-1, 0) };
        foreach ((int dx, int dy) in directions)
        {
            Tuple<int, int> newPos = new Tuple<int, int>(x + dx, y + dy);
            if (0 <= newPos.Item1 && newPos.Item1 < dungeon.GridSize &&
                0 <= newPos.Item2 && newPos.Item2 < dungeon.GridSize)
            {
                adjacent.Add(newPos);
            }
        }

        return adjacent;
    }

    private List<Tuple<int, int>> CreateGuaranteedPath(Dungeon dungeon, int length)
    {
        List<Tuple<int, int>> path = new List<Tuple<int, int>> { dungeon.entrance };
        HashSet<Tuple<int, int>> visited = new HashSet<Tuple<int, int>> { dungeon.entrance };
        Tuple<int, int> current = dungeon.entrance;

        for (int i = 0; i < length; i++)
        {
            List<Tuple<int, int>> adjacent = GetAdjacentPositions(dungeon, current);
            List<Tuple<int, int>> unvisited = adjacent.Where(pos => !visited.Contains(pos)).ToList();

            if (unvisited.Count == 0)
            {
                if (path.Count > 1)
                {
                    Tuple<int, int> removed = path[path.Count - 1];
                    path.RemoveAt(path.Count - 1);
                    visited.Remove(removed);
                    current = path[path.Count - 1];
                    continue;
                }
                else
                {
                    break;
                }
            }

            Tuple<int, int> nextPos = unvisited[random.Next(unvisited.Count)];
            path.Add(nextPos);
            visited.Add(nextPos);
            current = nextPos;
        }

        return path;
    }

    private void AddBonusRooms(Dungeon dungeon, double bonusProbability)
    {
        List<Tuple<int, int>> regularRooms = dungeon.grid.Keys
            .Where(pos => !pos.Equals(dungeon.entrance) && !pos.Equals(dungeon.exit))
            .ToList();

        if (regularRooms.Count == 0)
        {
            return;
        }

        int numBonus = Math.Max(1, (int)(regularRooms.Count * bonusProbability));
        List<Tuple<int, int>> bonusPositions = regularRooms
            .OrderBy(x => random.Next())
            .Take(Math.Min(numBonus, regularRooms.Count))
            .ToList();

        foreach (Tuple<int, int> pos in bonusPositions)
        {
            dungeon.grid[pos].IsBonus = true;
        }
    }
}

public class Dungeon
{
    public Dictionary<Tuple<int, int>, Room> grid = new Dictionary<Tuple<int, int>, Room>();
    public int FloorNumber { get; set; }
    public int PathLength { get; set; }
    public int GridSize { get; set; }
    public Tuple<int, int> entrance { get; set; }
    public Tuple<int, int> exit { get; set; }
    public List<Tuple<int, int>> mainPath { get; set; }
}

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

    public enum RoomType
    {
        Regular,
        Entrance,
        Exit,
        Bonus
    }
}
