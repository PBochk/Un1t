using UnityEngine;
using UnityEngine.Tilemaps;

public class OuterWallModel : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase wallTile;

    private Vector2Int size;
    private bool wasCreated;

    public void Awake()
    {
        static bool IsInteger(float numberValue)
            => numberValue == Mathf.Floor(numberValue);

        if (!IsInteger(transform.localScale.x))
            Debug.LogWarning("Wall's width isn't integer value");
        if (!IsInteger(transform.localScale.y))
            Debug.LogWarning("Wall's height isn't integer value");

        size = new Vector2Int((int)transform.localScale.x,
            (int)transform.localScale.y);

        if (size.x > 1 && size.y > 1)
            Debug.LogWarning("Wall has got uncorrect size");

        Create();

    }
    public void Create()
    {
        if (wasCreated)
        {
            Debug.LogWarning("Wall was created more than one time");
            return;
        }

        Vector3Int originPosition = tilemap.WorldToCell(transform.position);

        int startX = originPosition.x - size.x / 2;
        int startY = originPosition.y - size.y / 2;


        if (size.x >= size.y)
            for (int x = 0; x < size.x; x++)
                tilemap.SetTile(new Vector3Int(startX + x, startY, 0), wallTile);
        else
            for (int y = 0; y < size.y; y++)
                tilemap.SetTile(new Vector3Int(startX, startY + y, 0), wallTile);

        wasCreated = true;
    }


}
