public class PlayerExperienceModel
{
    private int level = 0;
    private int xpCoefficient = 25;
    private int currentXP;
    private int nextLevelXP = 25;

    public int Level => level;
    public float CurrentXP => currentXP;
    public float NextLevelXP => nextLevelXP;

    public PlayerExperienceModel(int level, int xpCoefficient)
    {
        this.level = level;
        this.xpCoefficient = xpCoefficient;
        SetNextLevelXP();
    }

    public void AddXP(int increment)
    {
        currentXP += increment;
        CheckXP();
    }

    private void CheckXP()
    {
        if (currentXP >= nextLevelXP)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        SetNextLevelXP();
    }

    private void SetNextLevelXP()
    {
        nextLevelXP = GetFibonachi(level) * xpCoefficient;
    }

    private int GetFibonachi(int n) => n > 1 ? GetFibonachi(n - 1) + GetFibonachi(n - 2) : n;
    
}