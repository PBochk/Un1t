using System;

public class PlayerMeleeWeaponModel : MeleeWeaponModel
{
    private float attackSpeed;
    private readonly float attackSpeedCap;
    private float attackCooldown;
    private float doubleHitChance;
    public float AttackSpeed
    {
        get => attackSpeed; 
        set
        {
            attackSpeed = value;
            attackCooldown = 1 / value;
        }
    }
    public float AttackCooldown => attackCooldown;
    public float DoubleHitChance => doubleHitChance;
    public PlayerMeleeWeaponModel(float damage, DamageType damageType, float attackSpeed, float doubleHitChance) : base(damage, damageType)
    {
        AttackSpeed = attackSpeed;
        attackSpeedCap = attackSpeed * 2;
        this.doubleHitChance = doubleHitChance;
    }

    public void UpgradeAttackSpeed(float increment)
    {
        AttackSpeed = AttackSpeed + increment < attackSpeed ? AttackSpeed + increment : attackSpeedCap;
    }

    public void UpgradeDamage(float increment)
    {
        Damage += increment;
    }

}