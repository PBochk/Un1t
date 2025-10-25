using UnityEngine;

public class EdgeOuterWallBuilder : OuterWallBuilder
{

    protected override void CheckSize(Direction direction, Vector2Int sizeTiles)
    {
        if ((direction == Direction.Horizontal && sizeTiles.y != 1) 
            || (direction == Direction.Vertical && sizeTiles.x != 1))
            Debug.LogWarning($"{direction} edge outer wall has got incorrect size: {sizeTiles.x}x{sizeTiles.y}");
    }
}
