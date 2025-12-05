using UnityEngine;

//TODO: use dictionaries, this solution is temporary
public static class EnemySelector
{
    public static EnemyController SelectEnemy(FloorEnemiesList enemiesList)
    {
        int greenWeight = FloorEnemiesList.GREEN_SLIME_FREQUENCY; 
        int redWeight = FloorEnemiesList.RED_SLIME_FREQUENCY; 
        int blueWeight = FloorEnemiesList.BLUE_SLIME_FREQUENCY;

        int totalWeight = greenWeight + redWeight + blueWeight;


        int randomValue = Random.Range(0, totalWeight);

        if (randomValue < greenWeight)
        {
            return enemiesList.Enemies[0];
        }
        else if (randomValue < greenWeight + redWeight)
        {
            return enemiesList.Enemies[1];
        }
        else
        {
            return enemiesList.Enemies[2];
        }
    }
}