//TODO: make melee weapon work
using UnityEngine;
using System.Collections.Generic;

public class MeleeWeaponController : MonoBehaviour
{
    [SerializeField] private LayerMask enemyMask;

    private Animator anim;
    private MeleeWeaponModel model;
    private CircleCollider2D weaponCollider;
    private ContactFilter2D contactFilter;
    private List<Collider2D> damagedEnemy;
    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
        model = GetComponent<MeleeWeaponModel>();
        weaponCollider = GetComponent<CircleCollider2D>();
        contactFilter = new();
        contactFilter.SetLayerMask(enemyMask);
    }

    //TODO: implement a coroutine or some other way to update cooldown
    private void FixedUpdate()
    {
        if (model.CurrentCooldown > 0)
        {
            model.CurrentCooldown -= Time.fixedDeltaTime;
        }
        
    }

    public void Attack()
    {
        if (model.CurrentCooldown <= 0)
        {
            Debug.Log("MeleeAttack");
            model.CurrentCooldown = model.AttackCooldown;
            damagedEnemy = new();
            anim.SetTrigger("MeleeAttack");
        }
    }

    // damagedEnemy list prevents enemy taking damage more than once per hit, but it's not a perfect solution performance wise
    //TODO: find a way to deal damage to enemy only once per attack
    public void OnAttack()
    {
        var enemies = new List<Collider2D>();
        Physics2D.OverlapCollider(weaponCollider, contactFilter, enemies);
        foreach (var enemy in enemies)
        {
            if (!damagedEnemy.Contains(enemy))
            {
                Debug.Log("Damage taken: " + model.Damage + " by enemy: " + enemy.name);
                damagedEnemy.Add(enemy);
            }
        }
    }
}
