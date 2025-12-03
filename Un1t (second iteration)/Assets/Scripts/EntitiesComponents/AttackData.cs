using UnityEngine;

public readonly struct AttackData 
{
    public float Damage { get; }
    public DamageType DamageType { get; }
    /// <summary>
    /// In order for a glitch enemy to decrease player's XP, this property must be greater than 0
    /// </summary>
    /// <remarks>
    /// This property equals 0 for every other entity
    /// </remarks>
    public float XPDamage { get; }
    /// <summary>
    /// Transform of entity that can push player 
    /// </summary>
    public Transform AttackerTransform { get; }
    public float PushSpeed { get; }

    public AttackData(float damage,
                      DamageType damageType = DamageType.Physical,
                      float xpDamage = 0,
                      Transform transform = null,
                      float pushSpeed = 0f)
    {
        Damage = damage;
        DamageType = damageType;
        XPDamage = xpDamage;
        AttackerTransform = transform;
        PushSpeed = pushSpeed;
    }
}
