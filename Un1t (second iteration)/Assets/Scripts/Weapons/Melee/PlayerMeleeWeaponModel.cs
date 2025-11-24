using System;

public class PlayerMeleeWeaponModel : MeleeWeaponModel
{
    private float attackSpeed;
    private readonly float attackSpeedCap;
    private float attackCooldown;
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

    public PlayerMeleeWeaponModel(float damage, DamageType damageType, float attackSpeed) : base(damage, damageType)
    {
        AttackSpeed = attackSpeed;
        attackSpeedCap = attackSpeed * 2;
    }

    public void UpgradeAttackSpeed(float increment)
    {
        AttackSpeed = AttackSpeed + increment < attackSpeed ? AttackSpeed + increment : attackSpeedCap;
    }

    public void UpgradeDamage()
    {
    }

}