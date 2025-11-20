using UnityEngine;

public readonly struct AttackData 
{
    public float Damage { get; }
    public DamageType DamageType { get; }
    public int XPDamage { get; } // is 0 for every entity except glitch

    public AttackData(float damage, DamageType damageType, int xpDamage = 0)
    {
        Damage = damage;
        DamageType = damageType;
        XPDamage = xpDamage;
    }
}
