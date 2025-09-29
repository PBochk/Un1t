using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeaponController : MonoBehaviour
{
    [SerializeField] private LayerMask enemyMask;

    protected MeleeWeaponModel model;
    private Collider2D[] weaponColliders;
    private ContactFilter2D contactFilter = new();
    private List<Collider2D> damagedEnemy;

    protected virtual void Awake()
    {
        model = GetComponent<MeleeWeaponModel>();
        weaponColliders = GetComponents<Collider2D>();
        contactFilter.SetLayerMask(enemyMask);
    }

    public virtual void StartMeleeAttack()
    {
        if (model.IsAttackReady)
        {
            damagedEnemy = new();
        }
    }

    //// damagedEnemy list prevents enemy taking damage more than once per hit, but it's not a perfect solution performance wise
    ////TODO: find a better way to deal damage to enemy only once per attack
    public virtual void OnMeleeAttack()
    {
        var enemies = new List<Collider2D>();
        foreach (var weaponCollider in weaponColliders)
        {
            Physics2D.OverlapCollider(weaponCollider, contactFilter, enemies);
            foreach (var enemy in enemies)
            {
                if (!damagedEnemy.Contains(enemy))
                {
                    Debug.Log("Damage taken: " + model.Damage + " by enemy " + enemy.name);
                    damagedEnemy.Add(enemy);
                }
            }
        }
    }
}

