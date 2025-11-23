using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OuterWallBuilder : TilesBuilder
{
    public const float TILE_SIZE = 1f; //TODO: remove this field;

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

    private const int SHURF_WIDTH = 2;
    private const int SHURF_DEPTHS = 3;

    private int thickness;
    private int length;
    private (int start, int end)[] emptyTilesForShurfesNumbersCouples;
    private bool wasShurfesCreated;

    public override void Create()
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

    //TODO: refactor this method.
    private void PlaceShurfes((int start, int end)[] emptyTilesForShurfesNumbers, Vector3 basePosition, Vector2 shurfFirstSideSize, Vector2 shurfSecondSideSize)
    {
        Direction shurfDirection;
        int directionMultiplier;

        float verticalPosition = 0f;
        float horizontalPosition = 0f;

        int shurfFirstSideThickness;
        int shurfSecondSideThickness;

        Vector2Int shurfGroundSize;
        Vector2 invisibleWallSize;
        Vector2 darknessSize;

        float invisibleWallOffset = (SHURF_DEPTHS / 2f + 0.5f) * TILE_SIZE;

        if (direction == Direction.Horizontal)
        {
            shurfDirection = Direction.Vertical;
            directionMultiplier = shurfsSpawnDirection == ShurfsSpawnDirection.Bottom ? -1 : 1;

            verticalPosition = basePosition.y + (SHURF_DEPTHS / 2f + thickness / 2f) * TILE_SIZE * directionMultiplier;

            shurfFirstSideThickness = (int)shurfFirstSideSize.x;
            shurfSecondSideThickness = (int)shurfSecondSideSize.x;

            invisibleWallSize = new Vector2((SHURF_WIDTH + shurfFirstSideThickness + shurfSecondSideThickness) * TILE_SIZE, TILE_SIZE);

            darknessSize = new (invisibleWallSize.x, invisibleWallSize.y*2);

            shurfGroundSize = new Vector2Int(SHURF_WIDTH, SHURF_DEPTHS + sizeTiles.y);
        }
        else
        {
            shurfDirection = Direction.Horizontal;
            directionMultiplier = shurfsSpawnDirection == ShurfsSpawnDirection.Left ? -1 : 1;

            horizontalPosition = basePosition.x + (SHURF_DEPTHS / 2f + thickness / 2f) * TILE_SIZE * directionMultiplier;

            shurfFirstSideThickness = (int)shurfFirstSideSize.y;
            shurfSecondSideThickness = (int)shurfSecondSideSize.y;

            invisibleWallSize = new Vector2(TILE_SIZE, (SHURF_WIDTH + +shurfFirstSideThickness + shurfSecondSideThickness) * TILE_SIZE);

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
                    basePosition.x + (shurfCenter - shurfFirstSideThickness - 0.5f) * TILE_SIZE,
                    verticalPosition);
                secondSidePosition = new Vector3(
                    basePosition.x + (shurfCenter + shurfSecondSideThickness + 0.5f) * TILE_SIZE,
                    verticalPosition);

                invisibleWallPosition = new Vector3(
                    basePosition.x + shurfCenter * TILE_SIZE,
                    verticalPosition + invisibleWallOffset * directionMultiplier
                );

                darknessPosition = invisibleWallPosition - new Vector3(0, directionMultiplier* 1.5f);

                shurfGroundPosition = new Vector3(
                    basePosition.x + shurfCenter * TILE_SIZE,
                    verticalPosition - sizeTiles.y*directionMultiplier/2f
                );
            }
            else
            {
                firstSidePosition = new Vector3(
                    horizontalPosition,
                    basePosition.y - (shurfCenter - shurfFirstSideThickness - 0.5f) * TILE_SIZE);
                secondSidePosition = new Vector3(
                    horizontalPosition,
                    basePosition.y - (shurfCenter + shurfSecondSideThickness + 0.5f) * TILE_SIZE);

                invisibleWallPosition = new Vector3(
                    horizontalPosition + invisibleWallOffset * directionMultiplier,
                    basePosition.y - (shurfCenter-1) * TILE_SIZE
                );


                darknessPosition = invisibleWallPosition - new Vector3(directionMultiplier*1.5f, 0);

                if (shurfFirstSideThickness != 1)
                    firstSidePosition -= new Vector3(0f, 1f);

                shurfGroundPosition = new Vector3(
                    horizontalPosition - sizeTiles.x * directionMultiplier / 2f,
                    basePosition.y - shurfCenter * TILE_SIZE
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

        BoxCollider2D collider = tile.GetComponent<BoxCollider2D>();

        if (thickness != 1 && direction == Direction.Horizontal)
        {
            collider.size = new Vector2(tileSize.x, 1f);
            collider.offset = new Vector2(0, 1f);
        }
        else
        {
            collider.size = tileSize;
            collider.offset = Vector2.zero;
        }
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