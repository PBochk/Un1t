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
    private PlayerRangeWeaponModel playerRangeWeaponModel;
    private bool isAttackReady = true;
    public PlayerRangeWeaponModel PlayerRangeWeaponModel => playerRangeWeaponModel;
    public bool IsAttackReady => isAttackReady;

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
