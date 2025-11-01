using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloorEnemiesList", menuName = "Scriptable Objects/FloorEnemiesList")]
public class FloorEnemiesList : ScriptableObject
{
    public IReadOnlyList<EnemyController> Enemies => enemies;

    [SerializeField] private EnemyController[] enemies;
}
