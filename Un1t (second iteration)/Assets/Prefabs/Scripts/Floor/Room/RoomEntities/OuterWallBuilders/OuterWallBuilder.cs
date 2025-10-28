using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OuterWallBuilder : MonoBehaviour
{
    public const float TILE_SIZE = 1f;

    //TODO: gain wall sprite of current level
    [SerializeField] protected OuterWallTiles WallTile;

    protected float thickness;
    protected Vector2Int sizeTiles;
    protected Direction direction;
    protected bool[] tilesAreEmpty;

    protected virtual void SetSize()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        sizeTiles = new Vector2Int((int)spriteRenderer.size.x, (int)spriteRenderer.size.y);

        spriteRenderer.enabled = false;

        if (sizeTiles.x > sizeTiles.y)
        {
            direction = Direction.Horizontal;
            tilesAreEmpty = new bool[sizeTiles.x];
            thickness = sizeTiles.y;
        }
        else
        {
            direction = Direction.Vertical;
            tilesAreEmpty = new bool[sizeTiles.y];
            thickness = sizeTiles.x;
        }

        CheckSize(direction, sizeTiles);
    }

    public void Create(params int[] emptyTilesNumbers)
    {
        SetSize();

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
                bool hasLeftHole = segmentStartIndex > 0;
                bool hasRightHole = i < tilesAreEmpty.Length;

                if (currentFragmentSize == 1)
                    CreateTile(WallTile.BasicWallTile, segmentStartIndex, currentFragmentSize, basePosition);
                else
                {
                    GameObject firstTilePrefab = hasLeftHole ? WallTile.PreviousAngleWallTile : WallTile.BasicWallTile;
                    CreateTile(firstTilePrefab, segmentStartIndex, 1, basePosition);

                    if (currentFragmentSize > 2)
                        CreateTile(WallTile.BasicWallTile, segmentStartIndex + 1, currentFragmentSize - 2, basePosition);

                    GameObject lastTilePrefab = hasRightHole ? WallTile.NextAngleWallTile : WallTile.BasicWallTile;
                    CreateTile(lastTilePrefab, segmentStartIndex + currentFragmentSize - 1, 1, basePosition);
                }

                currentFragmentSize = 0;
                segmentStartIndex = i + 1;
            }
            else if (!isCurrentFilled)
            {
                segmentStartIndex = i + 1;
            }
        }

    }

    private void CreateTile(GameObject tilePrefab, int startIndex, int fragmentSize, Vector3 basePosition)
    {
        GameObject tile = Instantiate(tilePrefab);
        SpriteRenderer tileRenderer = tile.GetComponent<SpriteRenderer>();

        Vector2 tileSize = direction == Direction.Horizontal
            ? new Vector2(fragmentSize, thickness)
            : new Vector2(thickness, fragmentSize);

        tileRenderer.size = tileSize;

        float centerOffset = TILE_SIZE * startIndex + TILE_SIZE * (fragmentSize - 1) / 2.0f;

        if (direction == Direction.Vertical)
            centerOffset = -centerOffset;

        tileRenderer.transform.position = direction == Direction.Horizontal
            ? new Vector3(basePosition.x + centerOffset, basePosition.y)
            : new Vector3(basePosition.x, basePosition.y + centerOffset);
    }

    protected enum Direction : sbyte { Vertical, Horizontal }

    protected virtual void CheckSize(Direction direction, Vector2Int sizeTiles) { }
}