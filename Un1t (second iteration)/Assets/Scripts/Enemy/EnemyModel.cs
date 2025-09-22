using UnityEngine;

/// <summary>
/// Used to determine enemy's current state,
/// intended to be immutable
/// </summary>
public struct EnemyModel
{
    public float Health { get; }
    public float MaxHealth { get; }
    public float Speed { get; }
    
    public EnemyModel(float health, float maxHealth, float speed)
    {
        Health = health;
        MaxHealth = maxHealth;
        Speed = speed;
    }
}