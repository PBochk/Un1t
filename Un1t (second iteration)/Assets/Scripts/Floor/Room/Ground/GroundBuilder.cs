using UnityEngine;

public abstract class GroundBuilder : TilesBuilder
{
    [SerializeField] protected GroundDecorationsTiles groundDecorationsTiles;

    public override void Create() { }

    public override void SetConfiguration()
    {
        SpriteRenderer groundRenderer = GetComponent<SpriteRenderer>();

        sizeTiles = new Vector2Int((int)groundRenderer.size.x, (int)groundRenderer.size.y);

        CheckSize(groundRenderer);

    }

}
