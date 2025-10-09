using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerRangeWeaponModel : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float ammo;
    [SerializeField] private float initialForce;

    public bool IsAttackReady { get; private set; } = true;
    public float InitialForce => initialForce;

    public IEnumerator WaitForAttackCooldown()
    {
        IsAttackReady = false;
        yield return new WaitForSeconds(attackCooldown);
        IsAttackReady = true;
    }
}
