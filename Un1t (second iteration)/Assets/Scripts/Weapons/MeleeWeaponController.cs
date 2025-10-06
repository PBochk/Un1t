using System.Collections;
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
    [SerializeField] private LayerMask targetMask; // I'm not sure if this should be here or in model
    [SerializeField] private Collider2D[] weaponColliders;

    protected MeleeWeaponModel model;
    private ContactFilter2D contactFilter = new();
    private HashSet<Collider2D> damagedTargets = new();

    /// <summary>
    /// Could be overriden with base call
    /// </summary>
    protected virtual void Awake()
    {
        model = GetComponent<MeleeWeaponModel>();
        contactFilter.SetLayerMask(targetMask);
    }

    /// <summary>
    /// Could be overriden with base call BEFORE model.IsAttackReady update
    /// </summary>
    protected virtual void StartMelee()
    {
        if (model.IsAttackReady)
        {
            damagedTargets.Clear();
        }
    }


    /// <summary>
    /// Should be called in the first active frame of attack animation
    /// </summary>
    protected virtual void StartMeleeActive()
    {
        model.IsAttackActive = true;
        StartCoroutine(OnMeleeActive());
    }

    /// <summary>
    /// Repeating every fixed update while attack is in active frames.
    /// Responsible for deciding which targets will be hit by attack.
    /// </summary>
    // damagedTargets list prevents enemy taking damage more than once per hit, but it's not a perfect solution performance wise
    // TODO: find a better way to deal damage to target only once per attack
    protected IEnumerator OnMeleeActive()
    {
        yield return new WaitForFixedUpdate();
        foreach (var weaponCollider in weaponColliders)
        {
            var targets = new List<Collider2D>();
            Physics2D.OverlapCollider(weaponCollider, contactFilter, targets);
            foreach (var target in targets)
            {
                if (!damagedTargets.Contains(target))
                {
                    target.GetComponent<HealthComponent>().TakeDamage(model.Damage);
                    damagedTargets.Add(target);
                }
            }
        }
        if (model.IsAttackActive)
        {
            StartCoroutine(OnMeleeActive());
        }
    }

    /// <summary>
    /// Should be called in the last active frame of attack animation
    /// </summary>
    protected virtual void EndMeleeActive()
    {
        model.IsAttackActive = false;
    }

}

