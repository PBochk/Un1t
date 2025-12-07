using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorEnemiesList", menuName = "Scriptable Objects/FloorEnemiesList")]
//TODO: Serialize Dictionaries, this solution is temporary
public class FloorEnemiesList : ScriptableObject
{
    public const int BLUE_SLIME_FREQUENCY = 4;
    public const int GREEN_SLIME_FREQUENCY = 2;
    public const int RED_SLIME_FREQUENCY = 6;
    public const int GLITCH_FREQUENCY = 1;

    public IReadOnlyList<GameObject> Enemies => enemies;

    [SerializeField] private GameObject[] enemies;


}
