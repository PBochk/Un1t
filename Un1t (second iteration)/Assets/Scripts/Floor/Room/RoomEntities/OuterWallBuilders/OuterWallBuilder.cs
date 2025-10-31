using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(SpriteRenderer))]
public class OuterWallBuilder : MonoBehaviour
{
    public const float TILE_SIZE = 1f;

    //TODO: gain wall sprite of current level.
    [SerializeField] protected OuterWallTiles wallTile;
    [SerializeField] protected GameObject shurfFirstSideTile;
    [SerializeField] protected GameObject shurfSecondSideTile;

    [SerializeField] protected ShurfsSpawnDirection shurfsSpawnDirection = 
        ShurfsSpawnDirection.Unidentified;

    protected float thickness;
    protected Vector2Int sizeTiles;
    protected Direction direction;
    protected bool[] tilesAreEmpty;

    private const int SHURFES_DEPTHS = 2;

        private void Awake()
    {
        Create(3);
    }

    public void Create(params int[] emptyTilesForShurfesNumbers)
    {
        SetConfiguration(emptyTilesForShurfesNumbers);

        Vector3 basePosition = transform.position - (direction == Direction.Horizontal
            ? new Vector3(TILE_SIZE * (sizeTiles.x - 1) / 2, 0)
            : new Vector3(0, -TILE_SIZE * (sizeTiles.y - 1) / 2));

        PlaceFragments(basePosition);

        if (shurfsSpawnDirection != ShurfsSpawnDirection.Unidentified)
            PlaceShurfes(emptyTilesForShurfesNumbers, basePosition);
    }


    protected virtual void SetConfiguration(int[] emptyTilesForShurfesNumbers)
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

        foreach (int emptyTileNumber in emptyTilesForShurfesNumbers)
            tilesAreEmpty[emptyTileNumber] = true;
    }


    private void PlaceFragments(in Vector3 basePosition)
    {
        int currentFragmentSize = 0;
        int segmentStartIndex = 0;

        for (var i = 0; i <= tilesAreEmpty.Length; i++)
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
                    CreateFragment(wallTile.BasicWallTile, currentFragmentSize, direction,
                        CalculateWallFragmentPosition(segmentStartIndex, currentFragmentSize, basePosition));
                else
                {
                    GameObject firstTilePrefab = hasLeftHole ? wallTile.PreviousCornerWallTile : wallTile.BasicWallTile;
                    CreateFragment(firstTilePrefab, 1, direction,
                        CalculateWallFragmentPosition(segmentStartIndex, 1, basePosition));

                    if (currentFragmentSize > 2)
                        CreateFragment(wallTile.BasicWallTile, currentFragmentSize - 2, direction,
                            CalculateWallFragmentPosition(segmentStartIndex+1, currentFragmentSize - 2, basePosition));

                    GameObject lastTilePrefab = hasRightHole ? wallTile.NextCornerWallTile : wallTile.BasicWallTile;
                    CreateFragment(lastTilePrefab, 1, direction,
                        CalculateWallFragmentPosition(segmentStartIndex + currentFragmentSize - 1, 1, basePosition));
                }

                currentFragmentSize = 0;
                segmentStartIndex = i + 1;
            }
            else if (!isCurrentFilled)
                segmentStartIndex = i + 1;
        }

    }

    private void PlaceShurfes(int[] emptyTilesForShurfesNumbers, in Vector3 basePosition)
    {
        foreach (int tileNumber in emptyTilesForShurfesNumbers)
        {
            Vector3 firstSidePosition;
            Vector3 secondSidePosition;
            Direction shurfDirection;
            if (direction == Direction.Horizontal)
            {
                float verticalPosition = basePosition.y + (TILE_SIZE * SHURFES_DEPTHS - TILE_SIZE/2) * (shurfsSpawnDirection == ShurfsSpawnDirection.Bottom
                    ? -1 : 1);

                firstSidePosition = new(basePosition.x + (tileNumber + 1) * TILE_SIZE, verticalPosition);
                secondSidePosition = new(basePosition.x + (tileNumber - 1) * TILE_SIZE, verticalPosition); 
                shurfDirection = Direction.Vertical;
            }
            else
            {
                float horizontalPosition = basePosition.x + (TILE_SIZE * SHURFES_DEPTHS - TILE_SIZE / 2) * (shurfsSpawnDirection == ShurfsSpawnDirection.Left
                  ? -1 : 1);

                firstSidePosition = new (horizontalPosition, basePosition.y + (tileNumber - 1) * TILE_SIZE);
                secondSidePosition = new (horizontalPosition, basePosition.y + (tileNumber + 1) * TILE_SIZE);
                shurfDirection = Direction.Horizontal;
            }

            CreateFragment(shurfFirstSideTile,  SHURFES_DEPTHS, shurfDirection, firstSidePosition);
            CreateFragment(shurfSecondSideTile, SHURFES_DEPTHS, shurfDirection, secondSidePosition);
        }
    }

    private void CreateFragment(GameObject tilePrefab, int fragmentSize, Direction direction, in Vector3 position)
    {
        GameObject tile = Instantiate(tilePrefab, transform);
        SpriteRenderer tileRenderer = tile.GetComponent<SpriteRenderer>();

        Vector2 tileSize = direction == Direction.Horizontal
            ? new Vector2(fragmentSize, thickness)
            : new Vector2(thickness, fragmentSize);

        tileRenderer.size = tileSize;

        tileRenderer.transform.position = position;
    }

    private Vector3 CalculateWallFragmentPosition(int startIndex, int fragmentSize, in Vector3 basePosition)
    {
        float centerOffset = TILE_SIZE * startIndex + TILE_SIZE * (fragmentSize - 1) / 2.0f;
        if (direction == Direction.Vertical)
            centerOffset = -centerOffset;
        return direction == Direction.Horizontal
            ? new Vector3(basePosition.x + centerOffset, basePosition.y)
            : new Vector3(basePosition.x, basePosition.y + centerOffset);
    }

    protected enum Direction : sbyte { Vertical, Horizontal }
    protected enum ShurfsSpawnDirection : sbyte {Unidentified, Top, Bottom, Left, Right }

    protected virtual void CheckSize(Direction direction, Vector2Int sizeTiles) { }
}