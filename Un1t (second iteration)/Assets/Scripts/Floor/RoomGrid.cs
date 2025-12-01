/// <summary>
/// Manages the grid of rooms in the floor
/// </summary>
public class RoomGrid
{
    public const int FLOOR_SIZE = 60;

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
