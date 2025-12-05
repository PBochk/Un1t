using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class TileConverter
{
    public static Tile[,] GetTileGrid(IEnumerable<GroundBuilder> groundBuilders, 
        IEnumerable<OuterWallBuilder> outerWallBuilders, Vector3 roomPosition)
    {
        Tile[,] allTiles = new Tile[RoomInfo.Size.y, RoomInfo.Size.x];

        foreach (GroundBuilder groundBuilder in groundBuilders)
        {
            Vector3 center = groundBuilder.transform.position - roomPosition;
            Vector2Int size = groundBuilder.SizeTiles;

            FillTileArea(allTiles, center, size, Tile.Ground);
        }

        foreach (OuterWallBuilder wallBuilder in outerWallBuilders)
        {
            wallBuilder.SetConfiguration();

            Vector3 center = wallBuilder.transform.position - roomPosition;
            Vector2Int size = wallBuilder.SizeTiles;

            Tile wallTileType = wallBuilder.CanCreateShurf ? Tile.ShurfableWall : Tile.UnshurfableWall;

            FillTileArea(allTiles, center, size, wallTileType);
        }

        return allTiles;
    }

    private static void FillTileArea(Tile[,] tiles, Vector3 center, Vector2Int size, Tile tileType)
    {
        int startX = Mathf.FloorToInt(center.x - size.x / 2f);
        int endX = startX + size.x - 1;

        int startY = Mathf.FloorToInt(center.y - size.y / 2f);
        int endY = startY + size.y - 1;

        for (var y = startY; y <= endY; y++)
        {
            for (var x = startX; x <= endX; x++)
            {
                if (x >= 0 && x < tiles.GetLength(1) && y >= 0 && y < tiles.GetLength(0))
                {
                    tiles[y, x] = tileType;
                }
            }
        }
    }

    /// <summary>
    /// For debug purpose only
    /// </summary>
    private static void DrawTileMap(Tile[,] tiles)
    {
        StringBuilder sb = new();

        for (int y = 0; y < tiles.GetLength(1); y++)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                Tile tileType = tiles[x, y];
                switch (tileType)
                {
                    case Tile.Ground:
                        sb.Append('G');
                        break;
                    case Tile.ShurfableWall:
                        sb.Append('B');
                        break;
                    case Tile.UnshurfableWall:
                        sb.Append('D');
                        break;
                    default:
                        sb.Append('O');
                        break;
                }
            }
            sb.AppendLine();
        }

        Debug.Log(sb.ToString());
    }
}
