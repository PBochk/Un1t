using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorEnemiesList", menuName = "Scriptable Objects/FloorEnemiesList")]
//TODO: Serialize Dictionaries, this solution is temporary
public class FloorEnemiesList : ScriptableObject
{
    public const int BLUE_SLIME_FREQUENCY = 1;
    public const int GREEN_SLIME_FREQUENCY = 2;
    public const int RED_SLIME_FREQUENCY = 3;

    public IReadOnlyList<EnemyController> Enemies => enemies;

    [SerializeField] private EnemyController[] enemies;
}
