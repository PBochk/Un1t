using UnityEngine;

public class StandardOuterWallModel : OuterWallBuilderModel
{
    [Header("Wall's parts are empty or filled")]
    [SerializeField] private bool firstPartIsEmpty;
    [SerializeField] private bool secondPartIsEmpty;
    [SerializeField] private bool thirdPartIsEmpty;

    public const int STANDARD_BASE_WALL_WIDTH = 16*3;
    public const int STANDARD_SIDE_WALL_HEIGHT = 9*3;


    protected override void Awake()
    {
        base.Awake();
        if (outerWallType != OuterWallType.Top && outerWallType != OuterWallType.Bottom
            && outerWallType != OuterWallType.Left && outerWallType != OuterWallType.Right)
            Debug.LogWarning($"Standard wall is not top, not bottom, not left, not right.");

        if (direction == Direction.Horizontal && sizeTiles.x != STANDARD_BASE_WALL_WIDTH 
            || sizeTiles.y != STANDARD_SIDE_WALL_HEIGHT)
            Debug.LogWarning($"Standard {direction} wall's size is incorrect.");

        bool[] parts = new bool[3] { firstPartIsEmpty, secondPartIsEmpty, thirdPartIsEmpty };
        int onePartSizeTiles = (direction == Direction.Horizontal ? sizeTiles.x : sizeTiles.y) / 3;

        for (var i = 0; i < parts.Length; i++)
        {
            if (parts[i])
                for (var j = onePartSizeTiles * i; j < onePartSizeTiles * (i + 1); j++)
                    tilesAreEmpty[j] = true;
        }
        Create();
    }
}
