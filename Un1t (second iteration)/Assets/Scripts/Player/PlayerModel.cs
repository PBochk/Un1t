using System;

public class PlayerModel
{
    private float maxHealth;
    public float MaxHealth => maxHealth;
    private float currentHealth;
    public float CurrentHealth => currentHealth;
    private float healthUpgrade;
    private float movingSpeed;
    public float MovingSpeed => movingSpeed;

    private int level;
    private int xpCoefficient;
    private int currentXP;
    private int nextLevelXP;
    public int Level => level;
    public float CurrentXP => currentXP;
    public float NextLevelXP => nextLevelXP;
    public event Action NextLevel;

    public PlayerModel(float maxHealth, float movingSpeed, int level, int xpCoefficient)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        this.movingSpeed = movingSpeed;
        this.level = level;
        this.xpCoefficient = xpCoefficient;
        SetNextLevelXP();
    }

    public void TakeHeal(float heal)
    {
        currentHealth += heal;
    }

    public void TakeDamage(float decrement)
    {
        currentHealth -= decrement;
    }

    public void UpgradeHealth()
    {
        maxHealth += healthUpgrade;
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
        NextLevel?.Invoke();
    }

    private void SetNextLevelXP()
    {
        nextLevelXP = GetFibonachi(level) * xpCoefficient;
    }

    private int GetFibonachi(int n) => n > 1 ? GetFibonachi(n - 1) + GetFibonachi(n - 2) : n;

}