using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerModelMB))]
public class PlayerHitable : Hitable
{
    [SerializeField] private float invulTime;
    private PlayerModel playerModel;
    private bool isVulnerable = true;
    /// <summary>
    /// If entity has dodge it ingores next hit
    /// </summary>
    //private bool hasDodge = false;

    private void Start()
    {
        playerModel = GetComponent<PlayerModelMB>().PlayerModel;
    }

    public override void TakeHit(AttackData attackData)
    {
        if (!isVulnerable) return;
        if (playerModel.DodgeChance >= Random.Range(0, 1f)) return;
        StartCoroutine(WaitForInvulnerability(invulTime));
        //if (hasDodge)
        //{
        //    hasDodge = false;
        //    return;
        //}
        base.TakeHit(attackData);
    }


    /// <summary>
    /// Method for starting player's invulnerability from external classes
    /// </summary>
    /// <param name="invulTime"></param>
    public void SetInvulForSeconds(float invulTime) => StartCoroutine(WaitForInvulnerability(invulTime));

    /// <summary>
    /// Makes player temporarily invulnerable
    /// </summary>
    private IEnumerator WaitForInvulnerability(float invulTime)
    {
        isVulnerable = false;
        yield return new WaitForSeconds(invulTime);
        isVulnerable = true;
    }

    /// <summary>
    /// 
    /// </summary>
    //public void SetDodgeActive()
    //{
    //    hasDodge = true;
    //}
}