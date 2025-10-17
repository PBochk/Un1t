using UnityEngine;

public readonly struct AttackData 
{
    public float Damage { get; }
    public DamageType DamageType { get; }
    //public GameObject Sender { get; }

    public AttackData(float damage, DamageType damageType)
    {
        Damage = damage;
        DamageType = damageType;
        //Sender = sender;
    }
}
