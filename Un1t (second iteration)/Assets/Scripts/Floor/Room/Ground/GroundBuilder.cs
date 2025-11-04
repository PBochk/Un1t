using UnityEngine;
using UnityEngine.UIElements;

public abstract class GroundBuilder : TilesBuilder
{
    [SerializeField] protected GroundTiles groundDecorationsTiles;

    public override void Create() 
    {
        GameObject tile = Instantiate(groundDecorationsTiles.GroundTile, transform);
        SpriteRenderer tileRenderer = tile.GetComponent<SpriteRenderer>();
        tileRenderer.size = sizeTiles;
        tileRenderer.enabled = false;
    }

    public override void SetConfiguration()
    {
        SpriteRenderer groundRenderer = GetComponent<SpriteRenderer>();

        sizeTiles = new Vector2Int((int)groundRenderer.size.x, (int)groundRenderer.size.y);

        CheckSize(groundRenderer);

    }

}
