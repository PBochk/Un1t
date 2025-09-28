using System.Collections.Generic;
using UnityEngine;


public abstract class MeleeWeaponController : MonoBehaviour
{
    [SerializeField] private LayerMask enemyMask;

    protected MeleeWeaponModel model;
    private Animator anim;
    private CircleCollider2D weaponCollider;
    private ContactFilter2D contactFilter = new();
    private List<Collider2D> damagedEnemy;

    protected virtual void Awake()
    {
        model = GetComponent<MeleeWeaponModel>();
        anim = GetComponentInParent<Animator>();
        weaponCollider = GetComponent<CircleCollider2D>();
        contactFilter.SetLayerMask(enemyMask);
    }

    ////TODO: implement a coroutine or some other way to update cooldown
    private void FixedUpdate()
    {
        if (model.CurrentCooldown > 0)
        {
            model.CurrentCooldown -= Time.fixedDeltaTime;
        }
    }

    public void StartMeleeAttack()
    {
        if (model.CurrentCooldown <= 0)
        {
            //Debug.Log("MeleeAttack");
            model.CurrentCooldown = model.AttackCooldown;
            damagedEnemy = new();
            anim.SetTrigger("MeleeAttack");
        }
    }

    //// damagedEnemy list prevents enemy taking damage more than once per hit, but it's not a perfect solution performance wise
    ////TODO: find a better way to deal damage to enemy only once per attack
    public void OnMeleeAttack()
    {
        var enemies = new List<Collider2D>();
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

