using System.Collections.Generic;
using Unity.Mathematics;
using System.Linq;

public static class ShurfesGenerator
{
    private const int MIN_SHURFES_COUNT = 0;
    private const int MAX_SHURFES_COUNT = 3;

    public static Dictionary<OuterWallBuilder, List<ShurfEmptyTilesPair>> SelectShurfesPositions
        (IReadOnlyList<OuterWallBuilder> shurfableWalls)
    {
        int shurfesCount = math.clamp(UnityEngine.Random.Range(MIN_SHURFES_COUNT, MAX_SHURFES_COUNT),
            0, shurfableWalls.Count);

        var shuffledShurfableWalls =
            from shurfableWall in shurfableWalls
            orderby UnityEngine.Random.value
            select shurfableWall;

        Dictionary<OuterWallBuilder, List<ShurfEmptyTilesPair>> shurfsPositions = new();

        foreach (OuterWallBuilder wall in shuffledShurfableWalls)
        {
            if (wall.Length / OuterWallBuilder.SHURF_WIDTH_WITH_NEIGHBOUR == 0) continue;

            bool[] wallTilesAreEmpty = new bool[wall.Length];

            shurfsPositions[wall] = new List<ShurfEmptyTilesPair>();

            if (wall.WallDirection == OuterWallBuilder.Direction.Vertical)
            {
                if (shurfesCount > 0)
                {
                    int minPosition = math.max(0, wall.Length / 2 - 2);
                    int maxPosition = math.min(wall.Length - OuterWallBuilder.SHURF_WIDTH, wall.Length / 2 + 1);

                    if (minPosition <= maxPosition)
                    {
                        int startPosition = UnityEngine.Random.Range(minPosition, maxPosition + 1);

                        if (startPosition + OuterWallBuilder.SHURF_WIDTH <= wall.Length)
                        {
                            wallTilesAreEmpty[startPosition] = true;
                            wallTilesAreEmpty[startPosition + 1] = true;

                            shurfsPositions[wall].Add(new ShurfEmptyTilesPair(startPosition, startPosition + 1));
                            shurfesCount--;
                        }
                    }
                }
            }
            else
            {
                int availableSlots = (wall.Length - OuterWallBuilder.SHURF_WIDTH_WITH_NEIGHBOUR)
                    / OuterWallBuilder.SHURF_WIDTH_WITH_NEIGHBOUR + 1;
                int possiblePairs = math.min(shurfesCount, availableSlots);

                var placedPairs = 0;
                for (var i = 1; i < wall.Length - 1 && placedPairs < possiblePairs; i += OuterWallBuilder.SHURF_WIDTH_WITH_NEIGHBOUR)
                {
                    if (i + OuterWallBuilder.SHURF_WIDTH <= wall.Length)
                    {
                        wallTilesAreEmpty[i] = true;
                        wallTilesAreEmpty[i + 1] = true;

                        shurfsPositions[wall].Add(new ShurfEmptyTilesPair(i, i + 1));
                        placedPairs++;
                        shurfesCount--;

                        if (shurfesCount == 0) break;
                    }
                }
            }

            if (shurfesCount == 0) break;
        }

        return shurfsPositions;
    }
}