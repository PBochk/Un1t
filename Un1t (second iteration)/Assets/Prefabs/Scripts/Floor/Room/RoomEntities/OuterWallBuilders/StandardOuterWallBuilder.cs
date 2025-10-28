using UnityEngine;

public class StandardOuterWallBuilder : OuterWallBuilder
{
    [Header("Wall's parts are empty or filled")]
    [SerializeField] private bool firstPartIsEmpty;
    [SerializeField] private bool middlePartIsEmpty;
    [SerializeField] private bool lastPartIsEmpty;

    public const int STANDARD_BASE_WALL_WIDTH = 18;
    public const int STANDARD_SIDE_WALL_HEIGHT = 9;

    private const int STANDARD_EDGE_WALL_THICKNESS = 1;

    protected override void SetSize()
    {
        base.SetSize();
        bool[] parts = new bool[3] { firstPartIsEmpty, middlePartIsEmpty, lastPartIsEmpty };
        int onePartSizeTiles = (direction == Direction.Horizontal ? sizeTiles.x : sizeTiles.y) / 3;

        for (var i = 0; i < parts.Length; i++)
            if (parts[i])
                for (var j = onePartSizeTiles * i; j < onePartSizeTiles * (i + 1); j++)
                    tilesAreEmpty[j] = true;
    }

    public void SetPartsEmptiness(RoomOuterWalls.Wall wall)
    {
        firstPartIsEmpty = wall.First.IsEmpty;
        middlePartIsEmpty = wall.Middle.IsEmpty;
        lastPartIsEmpty = wall.Last.IsEmpty;
    }

    protected override void CheckSize(Direction direction, Vector2Int sizeTiles)
    {
        if ((direction == Direction.Horizontal && (sizeTiles.x != STANDARD_BASE_WALL_WIDTH))           
            || (direction == Direction.Vertical 
            && (sizeTiles.y != STANDARD_SIDE_WALL_HEIGHT || (sizeTiles.x != STANDARD_EDGE_WALL_THICKNESS))))
                Debug.LogWarning($"{direction} standard outer wall has got incorrect size: {sizeTiles.x}x{sizeTiles.y}");
    }
}
