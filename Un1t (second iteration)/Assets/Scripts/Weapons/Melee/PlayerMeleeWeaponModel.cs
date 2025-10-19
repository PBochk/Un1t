public class PlayerMeleeWeaponModel : MeleeWeaponModel
{
    private float attackCooldown;
    public float AttackCooldown => attackCooldown;
    private float damageIncrement = 10f;
    private float attackCooldownUpgradeCoefficient = 0.9f;

    public PlayerMeleeWeaponModel(float damage, DamageType damageType, float attackCooldown) : base(damage, damageType)
    {
        this.attackCooldown = attackCooldown;
    }

    public void UpgradeDamage()
    {
        damage += damageIncrement;
        UpdateAttackData();
    }

    //Should be reworked with attack speed
    public void UpgradeAttackCooldown()
    {
        attackCooldown *= attackCooldownUpgradeCoefficient;
    }
}