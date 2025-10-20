using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(SpriteRenderer))]
public class OuterWallModel : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase wallTile;

    private Vector2Int size;
    private bool wasCreated;

    private const float TILE_SIZE = 0.24f;

    public void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        if (!IsCorrectSize(transform.localScale.x, out int width))
            Debug.LogWarning("Wall's width isn't multiple of 0.24", this);
        if (!IsCorrectSize(transform.localScale.y, out int height))
            Debug.LogWarning("Wall's height isn't multiple of 0.24", this);

        size = new Vector2Int(width, height);
        Create();
    }

    public void Create()
    {
        if (wasCreated)
        { 
            Debug.LogError("Wall was already created", this);
            return;
        }

        Vector3Int center = tilemap.WorldToCell(transform.position);

        int startX = center.x - size.x / 2;
        int startY = center.y - size.y / 2;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                tilemap.SetTile(new Vector3Int(startX + x, startY + y, 0), wallTile);
            }
        }

        wasCreated = true;
    }

    private static bool IsCorrectSize(float numberValue, out int integerDimension)
    {
        const float epsilon = 1e-5f;
        float tiles = numberValue / TILE_SIZE;
        integerDimension = Mathf.RoundToInt(tiles);
        float remainder = Mathf.Abs(tiles - integerDimension);
        return remainder < epsilon;
    }
}