using UnityEngine;

public abstract class TilesBuilder : MonoBehaviour
{
    public Vector2Int SizeTiles => sizeTiles;

    protected Vector2Int sizeTiles;
    public abstract void SetConfiguration();
    public abstract void Create();
    protected virtual void CheckSize(SpriteRenderer renderer) { }

}
