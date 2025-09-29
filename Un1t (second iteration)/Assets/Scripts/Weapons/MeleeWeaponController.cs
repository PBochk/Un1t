using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeaponController : MonoBehaviour
{
    [SerializeField] private LayerMask targetMask;

    protected MeleeWeaponModel model;
    private Collider2D[] weaponColliders;
    private ContactFilter2D contactFilter = new();
    private List<Collider2D> damagedTargets;

    protected virtual void Awake()
    {
        model = GetComponent<MeleeWeaponModel>();
        weaponColliders = GetComponents<Collider2D>();
        contactFilter.SetLayerMask(targetMask);
    }

    protected virtual void StartMeleeAttack()
    {
        if (model.IsAttackReady)
        {
            damagedTargets = new();
        }
    }

    //// damagedEnemy list prevents enemy taking damage more than once per hit, but it's not a perfect solution performance wise
    ////TODO: find a better way to deal damage to enemy only once per attack
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
                    //TODO: implement damage system
                    Debug.Log("Damage taken: " + model.Damage + " by enemy " + target.name);
                    damagedTargets.Add(target);
                }
            }
        }
    }
}

