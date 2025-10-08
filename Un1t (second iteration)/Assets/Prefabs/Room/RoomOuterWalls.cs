/// <summary>
/// Represents room's outer walls<br/>
/// Each of the 4 walls consists of 3 equal parts<br/>
/// Describes whether each of the parts is empty<br/>
/// <br/>
/// <code>
///    RoomOuterWalls.WALL.wallPart
///
///        first middle  last        
///       |----- ------ -----|       
///first  |        TOP       | first 
///       |                  |       
///       |L                R|       
///       |E                I|       
///middle |F                G| middle
///       |T                H|       
///       |                 T|       
///last   |      BOTTOM      | last  
///       |----- ------ -----|       
///        first middle  last   
/// </code>
/// </summary>
public readonly struct RoomOuterWalls
{
    public Wall Top { get; }
    public Wall Bottom { get; }
    public Wall Left { get; }
    public Wall Right { get; }


    public RoomOuterWalls(Wall top, Wall bottom, Wall left, Wall right)
    {
        Top = top;
        Bottom = bottom;
        Left = left;
        Right = right;
    }

    public readonly struct Wall
    {
        public WallPart First { get; }
        public WallPart Middle { get; }
        public WallPart Last { get; }

        public Wall(WallPart firstPart, WallPart middlePart, WallPart lastPart)
        {
            First = firstPart;
            Middle = middlePart;
            Last = lastPart;
        }


        public readonly struct WallPart
        {
            public bool IsEmpty { get; }

            public WallPart(bool isEmpty)
            { IsEmpty = isEmpty; }

        }

    }

}
