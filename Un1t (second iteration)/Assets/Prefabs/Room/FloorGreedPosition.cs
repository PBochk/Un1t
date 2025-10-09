/// <summary>
/// Represents room's position on the floor's greed from top left angle
/// </summary>
public readonly struct FloorGridPosition
{
    public static FloorGridPosition[] Directions { get; }
        
    public int X { get; }
    public int Y { get; }
    public FloorGridPosition(int x, int y)
    {
        X = x; 
        Y = y;
    }

    static FloorGridPosition()
    {
        Directions = new FloorGridPosition[] { new(0, -1), new(0, 1), new(-1, 0), new(1, 0) };
    }

    public static FloorGridPosition operator + (FloorGridPosition first, FloorGridPosition second)
    {
        return new FloorGridPosition(first.X + second.X, first.Y + second.Y);
    }

    public static implicit operator UnityEngine.Vector2Int(FloorGridPosition vector2Int)
    {
        return new UnityEngine.Vector2Int(vector2Int.X, vector2Int.Y);
    }
}