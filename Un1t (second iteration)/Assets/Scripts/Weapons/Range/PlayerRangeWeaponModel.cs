public class PlayerRangeWeaponModel
{
    private float damage;
    private float lifetime; //idk if this should be in native model
    private float initialForce;
    private float attackCooldown;
    private int ammo;

    public float Damage => damage;
    public float Lifetime => lifetime;
    public float InitialForce => initialForce;
    public float AttackCooldown => attackCooldown;
    public int Ammo => ammo;

    public PlayerRangeWeaponModel(float damage, float lifetime, float attackCooldown, int ammo)
    {
        this.damage = damage;
        this.lifetime = lifetime;
        this.attackCooldown = attackCooldown;
        this.ammo = ammo;
    }
}