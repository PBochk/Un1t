using UnityEngine;

public struct EnemyModel
{
    public float Health { get; }
    public float MaxHealth { get; }
    public float Speed { get; }
    public float Damage { get; }
    
    public EnemyModel(float health, float maxHealth, float speed, float damage)
    {
        Health = health;
        MaxHealth = maxHealth;
        Speed = speed;
        Damage = damage;
    }
    
    public EnemyModel WithHealth(float health)
    {
        return new EnemyModel(MaxHealth, health, Speed, Damage);
    }
    
    public EnemyModel WithMaxHealth(float maxHealth)
    {
        return new EnemyModel(maxHealth, Health, Speed, Damage);
    }
    
    public EnemyModel WithDamage(float damage)
    {
        return new EnemyModel(MaxHealth, Health, Speed, damage);
    }
    
    public EnemyModel WithSpeed(float speed)
    {
        return new EnemyModel(MaxHealth, Health, speed, Damage);
    }
}