using UnityEngine;

public class Floor1GroundBuilder : GroundBuilder
{
    private const float SMALL_DECORATION_DENSITY = 0.1f;
    private const float LARGE_DECORATION_DENSITY = 0.05f;
    private const int LARGE_DECORATION_SIZE = 4;

    public override void Create()
    {
        base.Create();

        GenerateDecorations();
    }

    //TODO: remade generation algorithm to remove magic numbers.
    private void GenerateDecorations()
    {
        bool[,] tileGrid = new bool[sizeTiles.x, sizeTiles.y];
        Vector3 topLeftPosition = transform.position - new Vector3(sizeTiles.x / 2, sizeTiles.y / 2);

        GenerateLargeDecorations(tileGrid, topLeftPosition + new Vector3(2, 1));
        GenerateSmallDecorations(tileGrid, topLeftPosition + new Vector3(0.5f, 0));
    }

    private void GenerateLargeDecorations(bool[,] largeOccupiedGrid, Vector3 topLeftPosition)
    {
        if (groundDecorationsTiles.LargeDecorationTiles.Count == 0) return;

        int decorationsPlaced = 0;
        int maxDecorations = GetTargetLargeDecorationCount();

        for (var x = 0; x <= sizeTiles.x - LARGE_DECORATION_SIZE && decorationsPlaced < maxDecorations; x++)
        {
            for (var y = 0; y <= sizeTiles.y - LARGE_DECORATION_SIZE && decorationsPlaced < maxDecorations; y++)
            {
                if (IsAreaFree(x, y, LARGE_DECORATION_SIZE, LARGE_DECORATION_SIZE, largeOccupiedGrid) &&
                    Random.value < LARGE_DECORATION_DENSITY)
                {
                    PlaceLargeDecoration(x, y, topLeftPosition);
                    MarkAreaOccupied(x, y, LARGE_DECORATION_SIZE, LARGE_DECORATION_SIZE, largeOccupiedGrid);
                    decorationsPlaced++;
                }
            }
        }
    }

    private void GenerateSmallDecorations(bool[,] smallOccupiedGrid, Vector3 topLeftPosition)
    {
        if (groundDecorationsTiles.SmallDecorationTiles.Count == 0) return;

        int decorationsPlaced = 0;
        int targetCount = GetTargetSmallDecorationCount();

        for (var x = 0; x < sizeTiles.x && decorationsPlaced < targetCount; x++)
        {
            for (var y = 0; y < sizeTiles.y && decorationsPlaced < targetCount; y++)
            {
                if (!smallOccupiedGrid[x, y] && Random.value < SMALL_DECORATION_DENSITY)
                {
                    PlaceSmallDecoration(x, y, topLeftPosition);
                    smallOccupiedGrid[x, y] = true;
                    decorationsPlaced++;
                }
            }
        }
    }

    private void PlaceLargeDecoration(int gridX, int gridY, Vector3 bottomLeftPosition)
    {
        GameObject decorationPrefab =
            groundDecorationsTiles.LargeDecorationTiles[Random.Range(0, groundDecorationsTiles.LargeDecorationTiles.Count)];

        GameObject decoration = Instantiate(decorationPrefab, bottomLeftPosition + new Vector3(gridX, gridY), Quaternion.identity, transform);
        decoration.name = $"{decoration.name}({gridX}x{gridY})";
    }

    private void PlaceSmallDecoration(int gridX, int gridY, Vector3 bottomLeftPosition)
    {
        GameObject decorationPrefab =
            groundDecorationsTiles.SmallDecorationTiles[Random.Range(0, groundDecorationsTiles.SmallDecorationTiles.Count)];

        GameObject decoration = Instantiate(decorationPrefab, bottomLeftPosition + new Vector3(gridX, gridY), Quaternion.identity, transform);
        decoration.name = $"{decoration.name}({gridX}x{gridY})";
    }

    private bool IsAreaFree(int startX, int startY, int width, int height, bool[,] occupiedGrid)
    {
        for (var x = startX; x < startX + width; x++)
            for (var y = startY; y < startY + height; y++)
                if (x >= sizeTiles.x || y >= sizeTiles.y || occupiedGrid[x, y])
                    return false;
        return true;
    }

    private void MarkAreaOccupied(int startX, int startY, int width, int height, bool[,] occupiedGrid)
    {
        for (var x = startX; x < startX + width; x++)
            for (var y = startY; y < startY + height; y++)
                if (x < sizeTiles.x && y < sizeTiles.y)
                    occupiedGrid[x, y] = true;
    }

    private int GetTargetSmallDecorationCount() =>
        Mathf.RoundToInt(sizeTiles.x * sizeTiles.y * SMALL_DECORATION_DENSITY);

    private int GetTargetLargeDecorationCount()
    {
        int maxPossible = (sizeTiles.x - LARGE_DECORATION_SIZE + 1) * (sizeTiles.y - LARGE_DECORATION_SIZE + 1);
        return Mathf.RoundToInt(maxPossible * LARGE_DECORATION_DENSITY);
    }
}