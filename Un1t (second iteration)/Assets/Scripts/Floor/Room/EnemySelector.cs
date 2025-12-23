using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EnemySelector
{
    public static GameObject SelectEnemy(FloorEnemiesList enemiesList)
    {
        IReadOnlyDictionary<GameObject, int> enemiesWithFrequencies = enemiesList.EnemiesWithFrequencies;

        int totalWeight = enemiesWithFrequencies.Values.Sum();

        int randomValue = Random.Range(0, totalWeight);
        var currentWeight = 0;

        foreach (KeyValuePair<GameObject, int> enemyWithFrequency in enemiesWithFrequencies)
        {
            currentWeight += enemyWithFrequency.Value;
            if (randomValue < currentWeight)
                return enemyWithFrequency.Key;

        }

        throw new System.Exception("Enemy wasn't selected");
    }
}