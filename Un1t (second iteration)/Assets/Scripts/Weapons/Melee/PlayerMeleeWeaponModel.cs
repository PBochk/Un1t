using System.Collections;
using UnityEngine;

/// <summary>
/// An implementation of weapon model for player to use
/// </summary>
public class PlayerMeleeWeaponModel : MeleeWeaponModel
{
    [SerializeField] private float attackCooldown;
    // private WaitForSeconds waitForCooldown; 

    //private void Awake()
    //{
    //    //waitForCooldown = new WaitForSeconds(attackCooldown); idk why but it works like attack cooldown is 0
    //    Debug.Log(attackCooldown);
    //}

    /// <summary>
    /// Is used when attack ends. Waits for attack cooldown than allows to make the next one  
    /// </summary>
    public IEnumerator WaitForAttackCooldown()
    {
        isAttackReady = false;
        //yield return waitForCooldown;
        yield return new WaitForSeconds(attackCooldown);
        isAttackReady = true;
    }
}
