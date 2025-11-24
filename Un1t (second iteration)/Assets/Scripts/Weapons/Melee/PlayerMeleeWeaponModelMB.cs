using System.Collections;
using UnityEngine;

/// <summary>
/// An implementation of weapon model for player to use
/// </summary>
public class PlayerMeleeWeaponModelMB : MeleeWeaponModelMB
{
    [SerializeField] private float attackSpeed;

    protected override void Awake()
    {
        base.Awake();
        meleeWeaponModel = new PlayerMeleeWeaponModel(damage, damageType, attackSpeed);
    }

    /// <summary>
    /// Is used when attack ends. Waits for attack cooldown than allows to make the next one  
    /// </summary>
    public IEnumerator WaitForAttackCooldown()
    {
        isAttackReady = false;
        yield return new WaitForSeconds(((PlayerMeleeWeaponModel)meleeWeaponModel).AttackCooldown);
        isAttackReady = true;
    }
}
