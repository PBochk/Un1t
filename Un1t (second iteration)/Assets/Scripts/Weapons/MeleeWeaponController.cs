using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract class made for summarizing general behaviour of melee weapon attacks.
/// </summary>
/// <remarks>
/// Its derivatives should subscribe its methods on parent's (player or enemy) controller events
/// and invoke changes in model.IsAttackReady
/// </remarks>
public abstract class MeleeWeaponController : MonoBehaviour
{
    [SerializeField] private LayerMask targetMask;

    protected MeleeWeaponModel model;
    private Collider2D[] weaponColliders;
    private ContactFilter2D contactFilter = new();
    private HashSet<Collider2D> damagedTargets = new();

    /// <summary>
    /// Could be overriden with base call
    /// </summary>
    protected virtual void Awake()
    {
        model = GetComponent<MeleeWeaponModel>();
        weaponColliders = GetComponents<Collider2D>();
        contactFilter.SetLayerMask(targetMask);
    }

    /// <summary>
    /// Could be overriden with base call BEFORE model.IsAttackReady update
    /// </summary>
    protected virtual void StartMeleeAttack()
    {
        if (model.IsAttackReady)
        {
            damagedTargets.Clear();
        }
    }

    /// <summary>
    /// Responsible for deciding which targets will be hit by attack
    /// </summary>
    /// <remarks>
    /// damagedTargets list prevents enemy taking damage more than once per hit, but it's not a perfect solution performance wise
    /// </remarks>
    // TODO: find a better way to deal damage to target only once per attack
    protected virtual void OnMeleeAttack()
    {
        var targets = new List<Collider2D>();
        foreach (var weaponCollider in weaponColliders)
        {
            Physics2D.OverlapCollider(weaponCollider, contactFilter, targets);
            foreach (var target in targets)
            {
                if (!damagedTargets.Contains(target))
                {
                    target.GetComponent<HealthComponent>().TakeDamage(model.Damage);
                    Debug.Log("Damage taken: " + model.Damage + " by entity " + target.name);
                    damagedTargets.Add(target);
                }
            }
        }
    }
}

