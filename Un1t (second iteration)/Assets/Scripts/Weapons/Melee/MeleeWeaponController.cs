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

[RequireComponent(typeof(MeleeWeaponModelMB))]
public abstract class MeleeWeaponController : MonoBehaviour
{
    [SerializeField] protected LayerMask targetMask;
    [SerializeField] protected Collider2D[] weaponColliders;

    protected MeleeWeaponModelMB modelMB;
    protected MeleeWeaponModel model;
    protected ContactFilter2D contactFilter = new();
    protected HashSet<Collider2D> damagedTargets = new();
    protected WaitForFixedUpdate waitForFixedUpdate = new();

    /// <summary>
    /// Should be overriden with base call and modelMB assignment
    /// </summary>
    protected virtual void Awake()
    {
        contactFilter.SetLayerMask(targetMask);
    }

    protected virtual void Start()
    {
        model = modelMB.MeleeWeaponModel;
    }

    /// <summary>
    /// Could be overriden with base call BEFORE model.IsAttackReady update
    /// </summary>
    protected virtual void StartMelee()
    {
        //if (modelMB.IsAttackReady)
        //{
        //}
    }

    /// <summary>
    /// Should be called in the first active frame of attack animation
    /// </summary>
    protected virtual void StartMeleeActive()
    {
        modelMB.StartActive(); // Maybe it's better to subscribe model on some event, idk
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
        do
        {
            foreach (var weaponCollider in weaponColliders)
            {
                var targets = new List<Collider2D>();
                Physics2D.OverlapCollider(weaponCollider, contactFilter, targets);
                foreach (var target in targets)
                {
                    if (!damagedTargets.Contains(target))
                    {
                        var hittable = target.GetComponent<Hitable>();
                        hittable.TakeHit(model.AttackData);
                        damagedTargets.Add(target);
                    }
                }
            }
            yield return waitForFixedUpdate;
        }
        while (modelMB.IsAttackActive);
    }

    /// <summary>
    /// Should be called in the last active frame of attack animation
    /// </summary>
    protected virtual void EndMeleeActive()
    {
        modelMB.EndActive();
        damagedTargets.Clear();
    }
}

