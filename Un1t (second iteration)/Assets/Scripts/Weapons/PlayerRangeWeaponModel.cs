using System.Collections;
using UnityEngine;

public class PlayerRangeWeaponModel : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;
    [SerializeField] private LayerMask solid;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float ammo;
    [SerializeField] private float initialForce;
    public bool IsAttackReady { get; private set; } = true;
    public float InitialForce => initialForce;

    public float Damage { get => damage; set => damage = value; }
    public float Lifetime { get => lifetime; set => lifetime = value; }
    public LayerMask Solid { get => solid; }

    public IEnumerator WaitForAttackCooldown()
    {
        IsAttackReady = false;
        yield return new WaitForSeconds(attackCooldown);
        IsAttackReady = true;
    }
}
