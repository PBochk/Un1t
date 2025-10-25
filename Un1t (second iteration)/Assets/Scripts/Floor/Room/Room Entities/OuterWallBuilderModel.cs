using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class OuterWallBuilderModel : MonoBehaviour
{
    [SerializeField] protected GameObject wallTile;
    [SerializeField] protected OuterWallType outerWallType;

    public const float TILE_SIZE = 0.24f;

    private const int THICKNESS = 3;

    protected Vector2Int sizeTiles;
    protected Direction direction;
    protected bool[] tilesAreEmpty;

    private bool wasCreated;

    protected virtual void Awake()
    {
        static bool IsCorrectSize(float numberValue, out int integerDimension)
        {
            const float epsilon = 1e-5f;
            float tiles = numberValue / TILE_SIZE;
            integerDimension = Mathf.RoundToInt(tiles);
            float remainder = Mathf.Abs(tiles - integerDimension);
            return remainder < epsilon;
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        if (!IsCorrectSize(transform.localScale.x, out int width))
            Debug.LogWarning($"Wall's width isn't multiple of {TILE_SIZE}", this);
        if (!IsCorrectSize(transform.localScale.y, out int height))
            Debug.LogWarning($"Wall's height isn't multiple of {TILE_SIZE}", this);

        if (width != THICKNESS && height != THICKNESS)
            Debug.LogWarning("Wall has got incorrect size", this);

        if (width >= height)
        {
            direction = Direction.Horizontal;
            tilesAreEmpty = new bool[width];

            if (!(outerWallType == OuterWallType.Top 
                || outerWallType == OuterWallType.Bottom || outerWallType == OuterWallType.Angle))
                Debug.LogWarning("Horizontal wall's size is incorrect", this);
        }
        else
        {

            direction = Direction.Vertical;
            tilesAreEmpty = new bool[height];

            if (!(outerWallType == OuterWallType.Left 
                || outerWallType == OuterWallType.Right))
                Debug.LogWarning("Vertical wall's size is incorrect", this);
        }

        sizeTiles = new Vector2Int(width, height);

    }

    public void Create(params int[] emptyTilesNumbers)
    {
        if (wasCreated)
        {
            Debug.LogError("Wall was already created", this);
            return;
        }

        foreach (int emptyTileNumber in emptyTilesNumbers)
            tilesAreEmpty[emptyTileNumber] = true;


        Vector3 basePosition = transform.position - (direction == Direction.Horizontal
            ? new Vector3(TILE_SIZE * (sizeTiles.x - 1) / 2, 0)
            : new Vector3(0, -TILE_SIZE * (sizeTiles.y - 1) / 2));

        int currentFragmentSize = 0;
        int segmentStartIndex = 0;

        for (int i = 0; i <= tilesAreEmpty.Length; i++)
        {
            bool isCurrentFilled = (i < tilesAreEmpty.Length && !tilesAreEmpty[i]);
            bool shouldCreateTile = (i == tilesAreEmpty.Length) || !isCurrentFilled;

            if (isCurrentFilled)
                currentFragmentSize++;

            if (shouldCreateTile && currentFragmentSize > 0)
            {
                GameObject tile = Instantiate(wallTile);
                SpriteRenderer tileRenderer = tile.GetComponent<SpriteRenderer>();
                tile.transform.parent = transform;
                tile.GetComponent<BoxCollider2D>().size = tile.transform.localScale;

                Vector2 tileSize = direction == Direction.Horizontal
                    ? new Vector2(currentFragmentSize, THICKNESS)
                    : new Vector2(THICKNESS, currentFragmentSize);

                tileRenderer.size = tileSize;

                float centerOffset = TILE_SIZE * segmentStartIndex + TILE_SIZE * (currentFragmentSize - 1) / 2.0f;

                if (direction == Direction.Vertical)
                    centerOffset = -centerOffset;

                tileRenderer.transform.position = direction == Direction.Horizontal
                    ? new Vector3(basePosition.x + centerOffset, basePosition.y)
                    : new Vector3(basePosition.x, basePosition.y + centerOffset);

                currentFragmentSize = 0;
                segmentStartIndex = i + 1;
            }
            else if (!isCurrentFilled)
                segmentStartIndex = i + 1;
        }

        wasCreated = true;
    }
    protected enum OuterWallType : sbyte { Indefinite, Left, Right, Top, Bottom, Angle }

    protected enum Direction : sbyte { Vertical, Horizontal }
}
