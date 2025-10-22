using System;

public class PlayerMeleeWeaponModel : MeleeWeaponModel
{
    private float attackSpeed;
    private float damageIncrement;
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

    public PlayerMeleeWeaponModel(float damage, DamageType damageType, float attackSpeed, float damageIncrement) : base(damage, damageType)
    {
        AttackSpeed = attackSpeed;
        this.damageIncrement = damageIncrement;
    }

    public void UpgradeAttackSpeed(int level)
    {
        attackSpeed = (float)(0.9f * Math.Pow(1.1f, level));
    }

    public void UpgradeDamage()
    {
        Damage += damageIncrement;
    }

}