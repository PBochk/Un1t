using UnityEngine;

public readonly struct AttackData 
{
    public float Damage { get; }
    public DamageType DamageType { get; }
    public int XPDamage { get; } // is 0 for every entity except glitch
    public Transform AttackerTransform { get; }
    public float PushSpeed { get; }
    public AttackData(float damage, DamageType damageType, int xpDamage = 0, Transform transform = null, float pushSpeed = 0f)
    {
        Damage = damage;
        DamageType = damageType;
        XPDamage = xpDamage;
        AttackerTransform = transform;
        PushSpeed = pushSpeed;
    }
}
