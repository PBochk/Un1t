using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OuterWallBuilder : MonoBehaviour
{
    public const float TILE_SIZE = 1f;

    public bool CanCreateShurf => shurfsSpawnDirection != ShurfsSpawnDirection.Unidentified;
    public Direction WallDirection => direction;
    public Vector2Int SizeTiles => sizeTiles;

    public int Thickness => thickness;
    public int Length => length;


    [SerializeField] protected OuterWallTiles wallTile;
    [SerializeField] protected GameObject shurfFirstSideTile;
    [SerializeField] protected GameObject shurfSecondSideTile;

    [SerializeField] protected Direction direction;
    [SerializeField]
    protected ShurfsSpawnDirection shurfsSpawnDirection =
        ShurfsSpawnDirection.Unidentified;

    protected Vector2Int sizeTiles;
    protected bool[] tilesAreEmpty;

    private const int SHURFES_DEPTHS = 3;

    private int thickness;
    private int length;
    private (int start, int end)[] emptyTilesForShurfesNumbersCouples;
    private bool wasShurfesCreated;

    public void Create()
    {
        Vector3 basePosition = transform.position - (direction == Direction.Horizontal
            ? new Vector3(TILE_SIZE * (sizeTiles.x - 1) / 2, 0)
            : new Vector3(0, -TILE_SIZE * (sizeTiles.y - 1) / 2));

        PlaceFragments(basePosition);

        if (wasShurfesCreated)
        {
            SpriteRenderer shurfFirstSideRenderer = shurfFirstSideTile.GetComponent<SpriteRenderer>();
            SpriteRenderer shurfSecondSideRenderer = shurfSecondSideTile.GetComponent<SpriteRenderer>();

            PlaceShurfes(emptyTilesForShurfesNumbersCouples, basePosition, shurfFirstSideRenderer.size, shurfSecondSideRenderer.size);
        }
    }

    public virtual void SetConfiguration()
    {
        SpriteRenderer wallRenderer = GetComponent<SpriteRenderer>();

        sizeTiles = new Vector2Int((int)wallRenderer.size.x, (int)wallRenderer.size.y);

        wallRenderer.enabled = false;

        if (direction == Direction.Horizontal)
        {
            tilesAreEmpty = new bool[sizeTiles.x];
            thickness = sizeTiles.y;
            length = sizeTiles.x;
        }
        else
        {
            tilesAreEmpty = new bool[sizeTiles.y];
            thickness = sizeTiles.x;
            length = sizeTiles.y;
        }

        CheckSize();

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
                    CreateFragment(wallTile.BasicWallTile, currentFragmentSize, thickness, direction,
                        CalculateWallFragmentPosition(segmentStartIndex, currentFragmentSize, basePosition));
                else
                {
                    GameObject firstTilePrefab = hasLeftHole ? wallTile.PreviousCornerWallTile : wallTile.BasicWallTile;
                    CreateFragment(firstTilePrefab, 1, thickness, direction,
                        CalculateWallFragmentPosition(segmentStartIndex, 1, basePosition));

                    if (currentFragmentSize > 2)
                        CreateFragment(wallTile.BasicWallTile, currentFragmentSize - 2, thickness, direction,
                            CalculateWallFragmentPosition(segmentStartIndex + 1, currentFragmentSize - 2, basePosition));

                    GameObject lastTilePrefab = hasRightHole ? wallTile.NextCornerWallTile : wallTile.BasicWallTile;
                    CreateFragment(lastTilePrefab, 1, thickness, direction,
                        CalculateWallFragmentPosition(segmentStartIndex + currentFragmentSize - 1, 1, basePosition));
                }

                currentFragmentSize = 0;
                segmentStartIndex = i + 1;
            }
            else if (!isCurrentFilled)
                segmentStartIndex = i + 1;
        }
    }

    private void PlaceShurfes((int start, int end)[] emptyTilesForShurfesNumbers, Vector3 basePosition, Vector2 shurfFirstSideSize, Vector2 shurfSecondSideSize)
    {
        Direction shurfDirection;
        float verticalPosition = 0f;
        float horizontalPosition = 0f;
        int shurfFirstSideThickness;
        int shurfSecondSideThickness;

        if (direction == Direction.Horizontal)
        {
            shurfDirection = Direction.Vertical;
            verticalPosition = basePosition.y + (SHURFES_DEPTHS / 2f + thickness / 2f) * TILE_SIZE * (shurfsSpawnDirection == ShurfsSpawnDirection.Bottom
                ? -1 : 1);
            shurfFirstSideThickness = (int)shurfFirstSideSize.x;
            shurfSecondSideThickness = (int)shurfSecondSideSize.x;
        }
        else
        {
            shurfDirection = Direction.Horizontal;
            horizontalPosition = basePosition.x + (SHURFES_DEPTHS / 2f + thickness / 2f) * TILE_SIZE * (shurfsSpawnDirection == ShurfsSpawnDirection.Left
                ? -1 : 1);
            shurfFirstSideThickness = (int)shurfFirstSideSize.y;
            shurfSecondSideThickness = (int)shurfSecondSideSize.y;
        }

        foreach (float shurfCenter in emptyTilesForShurfesNumbers.Select(tileNumbersCouple => tileNumbersCouple.start + 0.5f))
        {
            Vector3 firstSidePosition;
            Vector3 secondSidePosition;

            if (direction == Direction.Horizontal)
            {
                firstSidePosition = new(basePosition.x + (shurfCenter - shurfFirstSideThickness - 0.5f) * TILE_SIZE, verticalPosition);
                secondSidePosition = new(basePosition.x + (shurfCenter + shurfSecondSideThickness + 0.5f) * TILE_SIZE, verticalPosition);
            }
            else
            {
                firstSidePosition = new(horizontalPosition, basePosition.y - (shurfCenter - shurfFirstSideThickness - 0.5f) * TILE_SIZE);
                secondSidePosition = new(horizontalPosition, basePosition.y - (shurfCenter + shurfSecondSideThickness + 0.5f) * TILE_SIZE);

                //TODO: rewrite algorithm and remove next two lines.
                if (shurfFirstSideThickness == 3)
                    firstSidePosition -= new Vector3(0f, 1f);
            }

            CreateFragment(shurfFirstSideTile, SHURFES_DEPTHS, shurfFirstSideThickness, shurfDirection, firstSidePosition);
            CreateFragment(shurfSecondSideTile, SHURFES_DEPTHS, shurfSecondSideThickness, shurfDirection, secondSidePosition);
        }
    }
            

    public void SetShurfesLocation(params (int start, int end)[] emptyTilesForShurfesNumbersCouples)
    {
        this.emptyTilesForShurfesNumbersCouples = emptyTilesForShurfesNumbersCouples;
        foreach ((int start, int end) in emptyTilesForShurfesNumbersCouples)
        {
            tilesAreEmpty[start] = true;
            tilesAreEmpty[end] = true;
        }
        wasShurfesCreated = true;
    }

    private void CreateFragment(GameObject tilePrefab, int fragmentSize, int thickness, Direction direction, in Vector3 position)
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

    public enum Direction : sbyte { Vertical, Horizontal }
    protected enum ShurfsSpawnDirection : sbyte { Unidentified, Top, Bottom, Left, Right }

    protected virtual void CheckSize() 
    {
        if (shurfsSpawnDirection == ShurfsSpawnDirection.Unidentified)
            return;

       bool incorrectHorizontalSpawn = direction == Direction.Horizontal &&
           !(shurfsSpawnDirection == ShurfsSpawnDirection.Top ||
              shurfsSpawnDirection == ShurfsSpawnDirection.Bottom);

        bool incorrectVerticalSpawn = direction == Direction.Vertical &&
            !(shurfsSpawnDirection == ShurfsSpawnDirection.Left ||
              shurfsSpawnDirection == ShurfsSpawnDirection.Right);

        if (incorrectHorizontalSpawn || incorrectVerticalSpawn)
            Debug.LogWarning($"{direction} outer wall shurfs spawn direction is {shurfsSpawnDirection}");
    }
}