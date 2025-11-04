using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GroundTiles", menuName = "Scriptable Objects/GroundTiles")]
public class GroundDecorationsTiles : ScriptableObject
{
    public IReadOnlyList<GameObject> SmallDecorationTiles => smallDecorationTiles;
    public IReadOnlyList<GameObject> LargeDecoratinTiles => largeDecorationTiles;

    [SerializeField] private GameObject[] smallDecorationTiles;
    [SerializeField] private GameObject[] largeDecorationTiles;
}
