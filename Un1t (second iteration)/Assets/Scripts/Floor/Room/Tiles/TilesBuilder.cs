using UnityEngine;

public abstract class TilesBuilder : MonoBehaviour
{
    public Vector2Int SizeTiles => sizeTiles;

    protected Vector2Int sizeTiles;
    public abstract void SetConfiguration();
    public abstract void Create();
    protected virtual void CheckSize(SpriteRenderer renderer)
    {
        if (renderer.size.x != sizeTiles.x || renderer.size.y != sizeTiles.y)
            Debug.LogWarning($"Outer wall has got nonintegral size: {renderer.size.x}x{renderer.size.y}");

    }

}
