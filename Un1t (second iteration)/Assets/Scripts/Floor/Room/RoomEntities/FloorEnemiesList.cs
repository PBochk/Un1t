using UnityEngine;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FloorEnemiesList", menuName = "Scriptable Objects/FloorEnemiesList")]
public class FloorEnemiesList : ScriptableObject
{
    public IReadOnlyDictionary<GameObject, int> EnemiesWithFrequencies => enemiesWithFrequencies;

    [SerializeField, SerializedDictionary("Enemies prefab", "Generation frequency")]
    private SerializedDictionary<GameObject, int> enemiesWithFrequencies = new();
}