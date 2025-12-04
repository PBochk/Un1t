using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMeleeWeaponConfig", menuName = "PlayerConfig/PlayerMeleeWeapon")]
public class PlayerMeleeWeaponConfig : ScriptableObject
{
    [SerializeField] private float damage;
    [SerializeField] private DamageType damageType;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float doubleHitChance;

    public float Damage => damage;
    public DamageType DamageType => damageType;
    public float AttackSpeed => attackSpeed;
    public float DoubleHitChance => doubleHitChance;
}
