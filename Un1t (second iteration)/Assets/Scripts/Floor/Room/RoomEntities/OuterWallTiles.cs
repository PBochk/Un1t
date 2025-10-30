using UnityEngine;

[CreateAssetMenu(fileName = "OuterWallTiles", menuName = "Scriptable Objects/OuterWallTiles")]
public class OuterWallTiles : ScriptableObject
{
    public GameObject BasicWallTile { get => basicWallTile; set => basicWallTile = value; }
    public GameObject PreviousCornerWallTile { get => previousCornerWallTile; set => previousCornerWallTile = value; }
    public GameObject NextCornerWallTile { get => nextCornerWallTile; set => nextCornerWallTile = value; }

    [SerializeField] private GameObject basicWallTile;
    [SerializeField] private GameObject previousCornerWallTile;
    [SerializeField] private GameObject nextCornerWallTile;
}
