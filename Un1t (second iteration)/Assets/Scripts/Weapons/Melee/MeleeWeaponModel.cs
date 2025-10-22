public class MeleeWeaponModel
{
    protected float damage;
    protected DamageType damageType;
    protected AttackData attackData;
    public float Damage
    {
        get => damage;
        protected set
        {
            damage = value;
            UpdateAttackData();
        }
    }
    public DamageType DamageType => damageType;
    public AttackData AttackData => attackData; 

    public MeleeWeaponModel(float damage, DamageType damageType)
    {
        Damage = damage;
        this.damageType = damageType;
    }

    public void UpdateAttackData()
    {
        attackData = new AttackData(damage, damageType);
    }

}