using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRangeWeaponModelMB : MonoBehaviour
{
    // TODO: implement ammo system
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int ammo;
    [SerializeField] private float initialForce;
    [SerializeField] private LayerMask solid;
    public LayerMask Solid => solid;
    public float InitialForce => initialForce;
    private bool isAttackReady = true;
    public bool IsAttackReady => isAttackReady;
    public PlayerRangeWeaponModel playerRangeWeaponModel;

    private void Awake()
    {
        playerRangeWeaponModel = new PlayerRangeWeaponModel(damage, lifetime, attackCooldown, ammo);
    }

    public IEnumerator WaitForAttackCooldown()
    {
        isAttackReady = false;
        yield return new WaitForSeconds(attackCooldown);
        isAttackReady = true;
    }
}
