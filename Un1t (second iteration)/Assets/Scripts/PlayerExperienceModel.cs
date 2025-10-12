using UnityEngine;
using UnityEngine.Events;

public class PlayerExperienceModel : MonoBehaviour
{
    private float level = 1;
    private float currentXP;
    private float lastLevelXP = 0;
    private float nextLevelXP = 25;

    public float Level => level;
    public float CurrentXP => currentXP;
    public float LastLevelXP => lastLevelXP; 
    public float NextLevelXP => nextLevelXP;

    public UnityEvent LevelUp;
    public void AddXP(float increment)
    {
        currentXP += increment;
        CheckXP();
    }

    private void CheckXP()
    {
        if (currentXP >= nextLevelXP)
        {
            level++;
            Debug.Log("Level up! Current level: " + level);
            SetNextLevelXP();
        }
    }

    private void SetNextLevelXP()
    {
        var old = lastLevelXP;
        lastLevelXP = nextLevelXP;
        nextLevelXP += old;
    }
}
