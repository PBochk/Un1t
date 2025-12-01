using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRangeWeaponModelMB : MonoBehaviour
{
    // TODO: remade with scriptable object
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int ammo;
    private PlayerRangeWeaponModel rangeWeaponModel;
    private bool isAttackReady = true;
    public PlayerRangeWeaponModel RangeWeaponModel => rangeWeaponModel;
    public bool IsAttackReady => isAttackReady;

    private void Awake()
    {
        rangeWeaponModel = new PlayerRangeWeaponModel(damage, lifetime, attackCooldown, ammo);
    }

    public IEnumerator WaitForAttackCooldown()
    {
        isAttackReady = false;
        yield return new WaitForSeconds(attackCooldown);
        isAttackReady = true;
    }
}
