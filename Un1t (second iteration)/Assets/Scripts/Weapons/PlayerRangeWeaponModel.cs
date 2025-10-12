using System.Collections;
using UnityEngine;

public class PlayerRangeWeaponModel : MonoBehaviour
{
    // TODO: implement ammo system
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;
    [SerializeField] private LayerMask solid;
    [SerializeField] private float initialForce;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float ammo;
    public float Damage => damage;
    public float Lifetime => lifetime;
    public LayerMask Solid => solid;
    public float InitialForce => initialForce;

    private bool isAttackReady = true;
    public bool IsAttackReady => isAttackReady;

    public IEnumerator WaitForAttackCooldown()
    {
        isAttackReady = false;
        yield return new WaitForSeconds(attackCooldown);
        isAttackReady = true;
    }
}
