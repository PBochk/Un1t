using UnityEngine;

public readonly struct ShurfEmptyTilesPair
{
    public int FirstTile { get; }
    public int SecondTile { get; }

    public float Center { get; }

    public ShurfEmptyTilesPair(int firstTile, int secondTile)
    {
        if (firstTile + 1 != secondTile)
            Debug.LogWarning($"Shurf empty tiles pair ({firstTile}, {secondTile}) is incorrect");
        FirstTile = firstTile;
        SecondTile = secondTile;
        Center = firstTile + 0.5f;
    }
}
