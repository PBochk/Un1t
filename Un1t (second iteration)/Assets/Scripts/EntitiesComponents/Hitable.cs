using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component that should be on everything that can react on weapon hits
/// </summary>
public class Hitable : MonoBehaviour
{
    [SerializeField] private HitableEntityType entityType;
    public HitableEntityType EntityType => entityType;
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
    public virtual void TakeHit(AttackData attackData)
    {
        HitTaken?.Invoke(attackData);
    }
}