using UnityEngine;

public static class EnemySelector
{
    public static GameObject SelectEnemy(FloorEnemiesList enemiesList)
    {
        int greenWeight = FloorEnemiesList.GREEN_SLIME_FREQUENCY;
        int blueWeight = FloorEnemiesList.BLUE_SLIME_FREQUENCY;
        int glitchWeight = FloorEnemiesList.GLITCH_FREQUENCY;

        int totalWeight = greenWeight + blueWeight + glitchWeight;

        int randomValue = Random.Range(0, totalWeight);

        if (randomValue < greenWeight)
        {
            return enemiesList.Enemies[0];
        }
        else if (randomValue < greenWeight + blueWeight)
        {
            return enemiesList.Enemies[2];
        }
        else
        {
            if (enemiesList.Enemies.Count > 3)
            {
                return enemiesList.Enemies[3];
            }
            else
                return enemiesList.Enemies[0];
        }
    }
}