using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OuterWallBuilder : TilesBuilder
{
    public const int SHURF_WIDTH_WITH_NEIGHBOUR  = SHURF_WIDTH + 2;
    public const int SHURF_WIDTH = 2;
    public const int SHURF_DEPTHS = 3;

    public bool CanCreateShurf => shurfsSpawnDirection != ShurfsSpawnDirection.Unidentified;
    public Direction WallDirection => direction;
    public Vector2 Position => transform.position;

    public int Thickness => thickness;
    public int Length => length;


    [SerializeField] protected OuterWallTiles wallTile;
    [SerializeField] protected GameObject shurfFirstSideTile;
    [SerializeField] protected GameObject shurfSecondSideTile;
    [SerializeField] protected GroundBuilder ground;

    [SerializeField] protected Direction direction;
    [SerializeField]
    protected ShurfsSpawnDirection shurfsSpawnDirection =
        ShurfsSpawnDirection.Unidentified;

    [SerializeField] private GameObject shurfDarkness;

    protected bool[] tilesAreEmpty;

    private int thickness;
    private int length;
    private IEnumerable<(int start, int end)> emptyTilesForShurfesNumbersCouples;
    private bool wasShurfesCreated;

    public override void Create()
    {
        Vector3 basePosition = transform.position - (direction == Direction.Horizontal
            ? new Vector3((sizeTiles.x - 1) / 2f, 0)
            : new Vector3(0, -(sizeTiles.y - 1) / 2f));

        PlaceFragments(basePosition);

        if (wasShurfesCreated)
        {
            SpriteRenderer shurfFirstSideRenderer = shurfFirstSideTile.GetComponent<SpriteRenderer>();
            SpriteRenderer shurfSecondSideRenderer = shurfSecondSideTile.GetComponent<SpriteRenderer>();

            PlaceShurfes(emptyTilesForShurfesNumbersCouples, basePosition, shurfFirstSideRenderer.size, shurfSecondSideRenderer.size);
        }
    }

    public override void SetConfiguration()
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

        CheckSize(wallRenderer);

    }

    private void PlaceFragments(in Vector3 basePosition)
    {
        var currentFragmentSize = 0;
        var segmentStartIndex = 0;

        int cornerTileSize = direction == Direction.Horizontal ? 1 : 3;

        for (var i = 0; i <= tilesAreEmpty.Length; i++)
        {
            bool isCurrentFilled = (i < tilesAreEmpty.Length) && !tilesAreEmpty[i];
            bool shouldCreateTile = (i == tilesAreEmpty.Length) || !isCurrentFilled;

            if (isCurrentFilled)
            {
                if (currentFragmentSize == 0)
                {
                    segmentStartIndex = i;
                }
                currentFragmentSize++;
            }

            if (shouldCreateTile && currentFragmentSize > 0)
            {
                bool hasLeftHole = segmentStartIndex > 0;
                bool hasRightHole = i < tilesAreEmpty.Length;

                if (currentFragmentSize == 1)
                {
                    GameObject tilePrefab;
                    var fragmentSizeForTile = 1;

                    if (hasLeftHole && hasRightHole)
                    {
                        tilePrefab = wallTile.BasicWallTile;
                    }
                    else if (hasLeftHole)
                    {
                        tilePrefab = wallTile.PreviousCornerWallTile;
                    }
                    else if (hasRightHole)
                    {
                        tilePrefab = wallTile.NextCornerWallTile;
                        fragmentSizeForTile = cornerTileSize;
                    }
                    else
                    {
                        tilePrefab = wallTile.BasicWallTile;
                    }

                    CreateFragment(tilePrefab, fragmentSizeForTile, thickness, direction,
                        CalculateWallFragmentPosition(segmentStartIndex, fragmentSizeForTile, basePosition));
                }
                else if (currentFragmentSize == 2)
                {
                    GameObject firstTilePrefab = hasLeftHole ? wallTile.PreviousCornerWallTile : wallTile.BasicWallTile;
                    int firstFragmentSize = 1; 
                    CreateFragment(firstTilePrefab, firstFragmentSize, thickness, direction,
                        CalculateWallFragmentPosition(segmentStartIndex, firstFragmentSize, basePosition));

                    GameObject secondTilePrefab = hasRightHole ? wallTile.NextCornerWallTile : wallTile.BasicWallTile;
                    int secondFragmentSize = hasRightHole ? cornerTileSize : 1; 
                    int secondStartIndex = segmentStartIndex + firstFragmentSize;

                    if (hasRightHole)
                    {
                        CreateFragment(secondTilePrefab, secondFragmentSize, thickness, direction,
                            CalculateWallFragmentPosition(secondStartIndex, secondFragmentSize, basePosition));
                    }
                    else
                    {
                        CreateFragment(secondTilePrefab, secondFragmentSize, thickness, direction,
                            CalculateWallFragmentPosition(secondStartIndex, secondFragmentSize, basePosition));
                    }
                }
                else
                {
                    GameObject firstTilePrefab = hasLeftHole ? wallTile.PreviousCornerWallTile : wallTile.BasicWallTile;
                    var firstFragmentSize = 1; 
                    CreateFragment(firstTilePrefab, firstFragmentSize, thickness, direction,
                        CalculateWallFragmentPosition(segmentStartIndex, firstFragmentSize, basePosition));

                    int middleSize = currentFragmentSize - 1 - (hasRightHole ? cornerTileSize : 1);
                    if (middleSize > 0)
                    {
                        int middleStartIndex = segmentStartIndex + 1;
                        CreateFragment(wallTile.BasicWallTile, middleSize, thickness, direction,
                            CalculateWallFragmentPosition(middleStartIndex, middleSize, basePosition));
                    }
                    if (hasRightHole)
                    {
                        int lastStartIndex = segmentStartIndex + currentFragmentSize - cornerTileSize;
                        CreateFragment(wallTile.NextCornerWallTile, cornerTileSize, thickness, direction,
                            CalculateWallFragmentPosition(lastStartIndex, cornerTileSize, basePosition));
                    }
                    else
                    {
                        int lastStartIndex = segmentStartIndex + currentFragmentSize - 1;
                        CreateFragment(wallTile.BasicWallTile, 1, thickness, direction,
                            CalculateWallFragmentPosition(lastStartIndex, 1, basePosition));
                    }
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

    //TODO: refactor this method.
    private void PlaceShurfes(IEnumerable<(int start, int end)> emptyTilesForShurfesNumbers, 
        Vector3 basePosition, Vector2 shurfFirstSideSize, Vector2 shurfSecondSideSize)
    {
        Direction shurfDirection;
        int directionMultiplier;

        var verticalPosition = 0f;
        var horizontalPosition = 0f;

        int shurfFirstSideThickness;
        int shurfSecondSideThickness;

        Vector2Int shurfGroundSize;
        Vector2 invisibleWallSize;
        Vector2 darknessSize;

        float invisibleWallOffset = SHURF_DEPTHS / 2f + 0.5f;

        if (direction == Direction.Horizontal)
        {
            shurfDirection = Direction.Vertical;
            directionMultiplier = shurfsSpawnDirection == ShurfsSpawnDirection.Bottom ? -1 : 1;

            verticalPosition = basePosition.y + (SHURF_DEPTHS / 2f + thickness / 2f) * directionMultiplier;

            shurfFirstSideThickness = (int)shurfFirstSideSize.x;
            shurfSecondSideThickness = (int)shurfSecondSideSize.x;

            invisibleWallSize = new Vector2(SHURF_WIDTH + shurfFirstSideThickness + shurfSecondSideThickness, 1f);

            darknessSize = new(invisibleWallSize.x, invisibleWallSize.y * 2);

            shurfGroundSize = new Vector2Int(SHURF_WIDTH, SHURF_DEPTHS + sizeTiles.y);
        }
        else
        {
            shurfDirection = Direction.Horizontal;
            directionMultiplier = shurfsSpawnDirection == ShurfsSpawnDirection.Left ? -1 : 1;

            horizontalPosition = basePosition.x + (SHURF_DEPTHS / 2f + thickness / 2f) * directionMultiplier;

            shurfFirstSideThickness = (int)shurfFirstSideSize.y;
            shurfSecondSideThickness = (int)shurfSecondSideSize.y;

            invisibleWallSize = new Vector2(1f, (SHURF_WIDTH + shurfFirstSideThickness + shurfSecondSideThickness));

            darknessSize = new(invisibleWallSize.x * 2, invisibleWallSize.y);

            shurfGroundSize = new Vector2Int(SHURF_DEPTHS + sizeTiles.x, SHURF_WIDTH);

        }

        foreach (float shurfCenter in emptyTilesForShurfesNumbers.Select(tileNumbersCouple => tileNumbersCouple.start + 0.5f))
        {
            Vector3 firstSidePosition;
            Vector3 secondSidePosition;

            Vector3 invisibleWallPosition;

            Vector3 darknessPosition;

            Vector3 shurfGroundPosition;

            if (direction == Direction.Horizontal)
            {
                firstSidePosition = new Vector3(
                    basePosition.x + (shurfCenter - shurfFirstSideThickness - 0.5f),
                    verticalPosition);
                secondSidePosition = new Vector3(
                    basePosition.x + (shurfCenter + shurfSecondSideThickness + 0.5f),
                    verticalPosition);

                invisibleWallPosition = new Vector3(
                    basePosition.x + shurfCenter,
                    verticalPosition + invisibleWallOffset * directionMultiplier
                );

                darknessPosition = invisibleWallPosition - new Vector3(0, directionMultiplier * 1.5f);

                shurfGroundPosition = new Vector3(
                    basePosition.x + shurfCenter,
                    verticalPosition - sizeTiles.y * directionMultiplier / 2f
                );
            }
            else
            {
                firstSidePosition = new Vector3(
                    horizontalPosition,
                    basePosition.y - (shurfCenter - shurfFirstSideThickness - 0.5f));
                secondSidePosition = new Vector3(
                    horizontalPosition,
                    basePosition.y - (shurfCenter + shurfSecondSideThickness + 0.5f));

                invisibleWallPosition = new Vector3(
                    horizontalPosition + invisibleWallOffset * directionMultiplier,
                    basePosition.y - (shurfCenter - 1)
                );


                darknessPosition = invisibleWallPosition - new Vector3(directionMultiplier * 1.5f, 0);

                if (shurfFirstSideThickness != 1)
                    firstSidePosition -= new Vector3(0f, 1f);

                shurfGroundPosition = new Vector3(
                    horizontalPosition - sizeTiles.x * directionMultiplier / 2f,
                    basePosition.y - shurfCenter
                );
            }

            CreateFragment(shurfFirstSideTile, SHURF_DEPTHS, shurfFirstSideThickness, shurfDirection, firstSidePosition);
            CreateFragment(shurfSecondSideTile, SHURF_DEPTHS, shurfSecondSideThickness, shurfDirection, secondSidePosition);

            GameObject invisibleWall = new("InvisibleShurfWall");
            invisibleWall.transform.SetParent(transform);
            BoxCollider2D collider = invisibleWall.AddComponent<BoxCollider2D>();
            collider.size = invisibleWallSize;
            invisibleWall.transform.position = invisibleWallPosition;

            GroundBuilder groundInstance = Instantiate(ground, shurfGroundPosition, Quaternion.identity, transform);
            groundInstance.SetSize(shurfGroundSize);
            groundInstance.Create();

            GameObject darkness = Instantiate(shurfDarkness, transform);
            darkness.GetComponent<SpriteRenderer>().size = darknessSize;
            darkness.transform.position = darknessPosition;
        }
    }


    public void SetShurfesLocation(IEnumerable<(int start, int end)> emptyTilesForShurfesNumbersCouples)
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

        BoxCollider2D collider = tile.GetComponent<BoxCollider2D>();

        if (thickness != 1 && direction == Direction.Horizontal)
        {
            collider.size = new Vector2(tileSize.x, tileSize.y - 0.2f);
            collider.offset = new Vector2(0, 0.1f);
        }
        else
        {
            collider.size = tileSize;
            collider.offset = Vector2.zero;
        }
    }

    private Vector3 CalculateWallFragmentPosition(int startIndex, int fragmentSize, in Vector3 basePosition)
    {
        float centerOffset = startIndex + (fragmentSize - 1) / 2.0f;
        if (direction == Direction.Vertical)
            centerOffset = -centerOffset;
        return direction == Direction.Horizontal
            ? new Vector3(basePosition.x + centerOffset, basePosition.y)
            : new Vector3(basePosition.x, basePosition.y + centerOffset);
    }

    public enum Direction : sbyte { Vertical, Horizontal }
    protected enum ShurfsSpawnDirection : sbyte { Unidentified, Top, Bottom, Left, Right }

    protected override void CheckSize(SpriteRenderer renderer)
    {
        base.CheckSize(renderer);
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
