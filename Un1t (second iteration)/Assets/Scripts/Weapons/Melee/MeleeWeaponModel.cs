public class MeleeWeaponModel
{
    protected float damage;
    protected DamageType damageType;
    protected AttackData attackData;
    public float Damage => damage;
    public DamageType DamageType => damageType;
    public AttackData AttackData => attackData; 

    public MeleeWeaponModel(float damage, DamageType damageType)
    {
        this.damage = damage;
        this.damageType = damageType;
        UpdateAttackData();
    }

    public void UpdateAttackData()
    {
        attackData = new AttackData(damage, damageType);
    }

}