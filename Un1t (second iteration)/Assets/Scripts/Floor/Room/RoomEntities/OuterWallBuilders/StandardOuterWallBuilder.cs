using UnityEngine;

public class StandardOuterWallBuilder : OuterWallBuilder
{
    [Header("Wall's parts are empty or filled")]
    [SerializeField] private bool firstPartIsEmpty;
    [SerializeField] private bool middlePartIsEmpty;
    [SerializeField] private bool lastPartIsEmpty;

    public const int STANDARD_BASE_WALL_WIDTH = 16;
    public const int STANDARD_SIDE_WALL_HEIGHT = 9;

    private const int STANDARD_EDGE_WALL_THICKNESS = 1;

    private static readonly int[] standardBaseWallPartsLengths = new int[] { 5, 6, 5 };
    private static readonly int[] standardSideWallPartsLengths = new int[] { 3, 3, 3 };

    public override void SetConfiguration()
    {
        base.SetConfiguration();
        bool[] parts = new bool[3] { firstPartIsEmpty, middlePartIsEmpty, lastPartIsEmpty };
        int[] lengths = direction == Direction.Horizontal ? standardBaseWallPartsLengths : standardSideWallPartsLengths;

        int wallTileIndex = 0;
        for (var i = 0; i < parts.Length; i++)
        {
            if (parts[i])
                for (var j = wallTileIndex; j < wallTileIndex + lengths[i]; j++)
                    if (j < tilesAreEmpty.Length)
                        tilesAreEmpty[j] = true;
            wallTileIndex += lengths[i];
        }
    }

    public void SetPartsEmptiness(RoomOuterWalls.Wall wall)
    {
        firstPartIsEmpty = wall.First.IsEmpty;
        middlePartIsEmpty = wall.Middle.IsEmpty;
        lastPartIsEmpty = wall.Last.IsEmpty;
    }

    protected override void CheckSize()
    {
        base.CheckSize();
        if ((direction == Direction.Horizontal && (sizeTiles.x != STANDARD_BASE_WALL_WIDTH))           
            || (direction == Direction.Vertical 
            && (sizeTiles.y != STANDARD_SIDE_WALL_HEIGHT || (sizeTiles.x != STANDARD_EDGE_WALL_THICKNESS))))
                Debug.LogWarning($"{direction} standard outer wall has got incorrect size: {sizeTiles.x}x{sizeTiles.y}");
    }
}
