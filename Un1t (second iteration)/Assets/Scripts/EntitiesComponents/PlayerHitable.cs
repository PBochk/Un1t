using System.Collections;
using UnityEngine;

public class PlayerHitable : Hitable
{
    [SerializeField] private float invulTime;
    private bool isVulnerable = true;
    /// <summary>
    /// Is entity has dodge it ingores next hit
    /// </summary>
    private bool hasDodge = false;


    public override void TakeHit(AttackData attackData)
    {
        if (!isVulnerable) return;
        StartCoroutine(WaitForInvulnerability(invulTime));
        if (hasDodge)
        {
            hasDodge = false;
            return;
        }
        base.TakeHit(attackData);
    }


    /// <summary>
    /// Method for starting entity's invulnerability from external classes
    /// </summary>
    /// <param name="invulTime"></param>
    public void SetInvulForSeconds(float invulTime) => StartCoroutine(WaitForInvulnerability(invulTime));

    /// <summary>
    /// Makes entity temporarily invulnerable
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
    public void SetDodgeActive()
    {
        hasDodge = true;
    }
}