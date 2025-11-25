public class ProjectileModel
{
    private float damage;
    private float lifetime;
    private AttackData attackData;
    public float Lifetime => lifetime;
    public AttackData AttackData => attackData;

    public ProjectileModel(float damage, float lifetime)
    {
        this.damage = damage;
        this.lifetime = lifetime;
        UpdateAttackData();
    }

    private void UpdateAttackData()
    {
        attackData = new AttackData(damage, DamageType.Physical);
    }

    public void UpgradeDamage(float increment)
    {
        damage += increment;
        UpdateAttackData();
    }
}