using System.Collections;
using UnityEngine;

public class PlayerMeleeWeaponModel : MeleeWeaponModel
{
    [SerializeField] private float damage;
    [SerializeField] public float AttackCooldown;

    private void Awake()
    {
        Damage = damage;
    }

    public IEnumerator WaitForAttackCooldown()
    {
        IsAttackReady = false;
        yield return new WaitForSeconds(AttackCooldown);
        IsAttackReady = true;
    }
}
