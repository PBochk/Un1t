using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundManager : MonoBehaviour
{
    private static readonly Vector2 positionOffset = new(0f, 0.5f);

    [SerializeField] private TileBase groundTile;
    [SerializeField] private TileBase[] decorationTiles;

    private Grid tileGrid;

    private Tilemap groundTilemap;
    private Tilemap decorationTilemap;

    public void CreateTileGrid()
    {
        GameObject gridObject = new("TileGrid");
        tileGrid = gridObject.AddComponent<Grid>();

        groundTilemap = CreateTilemap("Ground", tileGrid, positionOffset);
        decorationTilemap = CreateTilemap("Decoration", tileGrid, positionOffset);

        static Tilemap CreateTilemap(string name, Grid tileGrid, Vector3 offset)
        {
            GameObject tilemapObject = new($"{name}Tilemap");
            tilemapObject.transform.SetParent(tileGrid.transform);

            Tilemap tilemap = tilemapObject.AddComponent<Tilemap>();
            tilemapObject.AddComponent<TilemapRenderer>()
                .transform.position += offset;

            return tilemap;
        }
    }

}
