public class PlayerMeleeWeaponModel
{
    private float attackCooldown;
    public float AttackCooldown => attackCooldown;

    public PlayerMeleeWeaponModel(float attackCooldown)
    {
        this.attackCooldown = attackCooldown;
    }
}