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
    /// Called when gameobject takes hit from weapon or projectile
    /// </summary>
    /// <remarks>
    /// Entity's controller should subscribe on this event and process reaction on hit
    /// </remarks>
    public UnityEvent<AttackData> HitTaken;
    public void TakeHit(AttackData attackData)
    {
        if (!isVulnerable) return;
        HitTaken?.Invoke(attackData);
        StartCoroutine(WaitForInvulnerability());
    }

    /// <summary>
    /// Makes entity temporarily invulnerable
    /// </summary>
    private IEnumerator WaitForInvulnerability()
    {
        isVulnerable = false;
        yield return new WaitForSeconds(invulTime);
        isVulnerable = true;
    }
}