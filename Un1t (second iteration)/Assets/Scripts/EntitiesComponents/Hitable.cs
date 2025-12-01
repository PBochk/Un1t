using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component that should be on everything that can react on weapon hits
/// </summary>
public class Hitable : MonoBehaviour
{
    [SerializeField] private float invulTime;
    private bool isVulnerable = true;
    /// <summary>
    /// Is entity has dodge it ingores next hit
    /// </summary>
    private bool hasDodge = false;
    /// <summary>
    /// Invoked when gameobject takes hit from weapon or projectile
    /// </summary>
    /// <remarks>
    /// Entity's controller should subscribe on this event and process reaction on hit
    /// </remarks>
    public UnityEvent<AttackData> HitTaken;

    /// <summary>
    /// Called when entity receives a hit from damage source
    /// </summary>
    /// <param name="attackData"></param>
    public void TakeHit(AttackData attackData)
    {
        if (!isVulnerable) return;
        StartCoroutine(WaitForInvulnerability(invulTime));
        if (hasDodge)
        {
            hasDodge = false;
            return;
        }
        HitTaken?.Invoke(attackData);
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