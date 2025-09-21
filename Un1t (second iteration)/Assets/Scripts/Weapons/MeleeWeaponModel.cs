using UnityEngine;

public class MeleeWeaponModel : MonoBehaviour
{
    public float Damage;
    public float AttackCooldown;
    [HideInInspector] public float CurrentCooldown;
}
