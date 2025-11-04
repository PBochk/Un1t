using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component that should be on everything that can react on weapon hits
/// </summary>
public class Hitable : MonoBehaviour
{
    /// <summary>
    /// Called when gameobject takes hit from weapon or projectile
    /// </summary>
    /// <remarks>
    /// Entity's controller should subscribe on this event and process reaction on hit
    /// </remarks>
    public UnityEvent<AttackData> HitTaken;
    public void TakeHit(AttackData attackData)
    {
        HitTaken?.Invoke(attackData);
    }
}