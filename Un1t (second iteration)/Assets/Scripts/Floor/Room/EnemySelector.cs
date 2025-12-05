using UnityEngine;

//TODO: use dictionaries, this solution is temporary
public static class EnemySelector
{
    public static GameObject SelectEnemy(FloorEnemiesList enemiesList)
    {
        int greenWeight = FloorEnemiesList.GREEN_SLIME_FREQUENCY;
        int redWeight = FloorEnemiesList.RED_SLIME_FREQUENCY;
        int blueWeight = FloorEnemiesList.BLUE_SLIME_FREQUENCY;
        int glitchWeight = FloorEnemiesList.GLITCH_FREQUENCY;

        int totalWeight = greenWeight + redWeight + blueWeight + glitchWeight;

        int randomValue = Random.Range(0, totalWeight);

        if (randomValue < greenWeight)
        {
            if (enemiesList.Enemies.Count > 0)
                return enemiesList.Enemies[0];
        }
        else if (randomValue < greenWeight + redWeight)
        {
            if (enemiesList.Enemies.Count > 1)
                return enemiesList.Enemies[1];
        }
        else if (randomValue < greenWeight + redWeight + blueWeight)
        {
            if (enemiesList.Enemies.Count > 2)
                return enemiesList.Enemies[2];
        }
        else
        {
             return enemiesList.Enemies[3];
        }
        return enemiesList.Enemies[0];
    }
}