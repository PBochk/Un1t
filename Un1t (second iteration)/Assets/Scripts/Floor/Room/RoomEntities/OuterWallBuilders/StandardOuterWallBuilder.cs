using UnityEngine;

public class StandardOuterWallBuilder : OuterWallBuilder
{
    [Header("Wall's parts are empty or filled")]
    [SerializeField] private bool firstPartIsEmpty;
    [SerializeField] private bool secondPartIsEmpty;
    [SerializeField] private bool thirdPartIsEmpty;

    public const int STANDARD_BASE_WALL_WIDTH = 18;
    public const int STANDARD_SIDE_WALL_HEIGHT = 9;

    protected override void Awake()
    {
        base.Awake();
        bool[] parts = new bool[3] { firstPartIsEmpty, secondPartIsEmpty, thirdPartIsEmpty };
        int onePartSizeTiles = (direction == Direction.Horizontal ? sizeTiles.x : sizeTiles.y) / 3;

        for (var i = 0; i < parts.Length; i++)
            if (parts[i])
                for (var j = onePartSizeTiles * i; j < onePartSizeTiles * (i + 1); j++)
                    tilesAreEmpty[j] = true;

        Create();
    }

    protected override void CheckSize(Direction direction, Vector2Int sizeTiles)
    {
        if ((direction == Direction.Horizontal && (sizeTiles.x != STANDARD_BASE_WALL_WIDTH || sizeTiles.y != 1)) 
            || (direction == Direction.Vertical && (sizeTiles.y != STANDARD_SIDE_WALL_HEIGHT || sizeTiles.x != 1)))
            Debug.LogWarning($"{direction} edge outer wall has got incorrect size: {sizeTiles.x}x{sizeTiles.y}");
    }
}
