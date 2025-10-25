using UnityEngine;

[CreateAssetMenu(fileName = "OuterWallTiles", menuName = "Scriptable Objects/OuterWallTiles")]
public class OuterWallTiles : ScriptableObject
{
    public GameObject BasicWallTile { get => basicWallTile; set => basicWallTile = value; }
    public GameObject PreviousAngleWallTile { get => previousAngleWallTile; set => previousAngleWallTile = value; }
    public GameObject NextAngleWallTile { get => nextAngleWallTile; set => nextAngleWallTile = value; }

    [SerializeField] private GameObject basicWallTile;
    [SerializeField] private GameObject previousAngleWallTile;
    [SerializeField] private GameObject nextAngleWallTile;
}
