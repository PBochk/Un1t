using System.Collections;
using UnityEngine;

/// <summary>
/// An implementation of weapon model for player to use
/// </summary>
public class PlayerMeleeWeaponModel : MeleeWeaponModel
{
    [SerializeField] private float attackCooldown;

    /// <summary>
    /// Is used when attack ends. Waits for attack cooldown than allows to make the next one  
    /// </summary>
    public IEnumerator WaitForAttackCooldown()
    {
        isAttackReady = false;
        yield return new WaitForSeconds(attackCooldown);
        isAttackReady = true;
    }
}
