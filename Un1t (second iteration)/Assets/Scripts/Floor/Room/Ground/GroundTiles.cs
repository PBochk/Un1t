using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GroundTiles", menuName = "Scriptable Objects/GroundTiles")]
public class GroundTiles : ScriptableObject
{
    public GameObject GroundTile => groundTile;
    public IReadOnlyList<GameObject> SmallDecorationTiles => smallDecorationTiles;
    public IReadOnlyList<GameObject> LargeDecorationTiles => largeDecorationTiles;

    [SerializeField] private GameObject groundTile;
    [SerializeField] private GameObject[] smallDecorationTiles;
    [SerializeField] private GameObject[] largeDecorationTiles;
}
