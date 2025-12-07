using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRangeWeaponConfig", menuName = "PlayerConfig/PlayerRangeWeapon")]
public class PlayerRangeWeaponConfig : ScriptableObject
{
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int ammo;

    public float Damage => damage;
    public float Lifetime => lifetime;
    public float AttackCooldown => attackCooldown;
    public int Ammo => ammo;
}
