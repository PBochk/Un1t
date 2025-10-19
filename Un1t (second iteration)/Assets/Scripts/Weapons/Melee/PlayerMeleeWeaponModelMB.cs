using System.Collections;
using UnityEngine;

/// <summary>
/// An implementation of weapon model for player to use
/// </summary>
public class PlayerMeleeWeaponModelMB : MeleeWeaponModelMB
{
    [SerializeField] private float attackCooldown;
    public PlayerMeleeWeaponModel model;
    public PlayerMeleeWeaponModel PlayerMeleeWeaponModel => model;

    protected override void Awake()
    {
        base.Awake();
        model = new PlayerMeleeWeaponModel(damage, damageType, attackCooldown);
    }

    /// <summary>
    /// Is used when attack ends. Waits for attack cooldown than allows to make the next one  
    /// </summary>
    public IEnumerator WaitForAttackCooldown()
    {
        isAttackReady = false;
        yield return new WaitForSeconds(model.AttackCooldown);
        isAttackReady = true;
    }
}
