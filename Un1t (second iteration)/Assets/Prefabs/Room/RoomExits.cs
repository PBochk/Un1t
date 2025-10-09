/// <summary>
/// Represents information about exits from the room
/// If the wall contains at least one empty part, 
/// then there is an exit in this wall.
/// </summary>
public readonly struct RoomExits
{
    public bool HasTopWallExit { get; }
    public bool HasBottomWallExit { get; }
    public bool HasLeftWallExit { get; }
    public bool HasRightWallExit { get; }


    public RoomExits(bool hasTopWallExit, bool hasBottomWallExit, bool hasLeftWallExit, bool hasRightWallExit)
    {
        HasTopWallExit = hasTopWallExit;
        HasBottomWallExit = hasBottomWallExit;
        HasLeftWallExit = hasLeftWallExit;
        HasRightWallExit = hasRightWallExit;
    }

    public RoomExits(FloorGridPosition roomPosition, params FloorGridPosition[] emptyPositions)
    {
        HasTopWallExit = false;
        HasBottomWallExit = false;
        HasLeftWallExit = false;
        HasRightWallExit = false;

        foreach (FloorGridPosition emptyPosition in emptyPositions)
        {
            if (emptyPosition.X - roomPosition.X == 1)
                HasRightWallExit = true;
            else if (emptyPosition.X - roomPosition.X == -1)
                HasLeftWallExit = true;
            else if (emptyPosition.Y - roomPosition.Y == 1)
                HasBottomWallExit = true;
            else if (emptyPosition.Y - roomPosition.Y == 1)
                HasTopWallExit = true;
        }
    }



}