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

}