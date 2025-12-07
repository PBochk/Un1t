using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerRangeWeaponModelMB : MonoBehaviour
{
    [SerializeField] private PlayerRangeWeaponConfig config;
    private PlayerRangeWeaponModel rangeWeaponModel;
    private bool isAttackReady = true;
    public PlayerRangeWeaponModel RangeWeaponModel => rangeWeaponModel;
    public bool IsAttackReady => isAttackReady;

    private void Awake()
    {
        rangeWeaponModel = new PlayerRangeWeaponModel(config.Damage, config.Lifetime, config.AttackCooldown, config.Ammo);
    }

    public IEnumerator WaitForAttackCooldown()
    {
        isAttackReady = false;
        yield return new WaitForSeconds(config.AttackCooldown);
        isAttackReady = true;
    }
}
