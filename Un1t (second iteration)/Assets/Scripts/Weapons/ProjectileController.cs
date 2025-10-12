using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ProjectileModel))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask solid;
    private ProjectileModel model;
    private Collider2D projectileCollider;
    private ContactFilter2D contactFilter;

    private void Awake()
    {
        projectileCollider = GetComponent<Collider2D>();
        model = GetComponent<ProjectileModel>();
        contactFilter.SetLayerMask(solid);
        StartCoroutine(DestroyProjectile());
    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(model.Lifetime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var targets = new List<Collider2D>();
        Physics2D.OverlapCollider(projectileCollider, contactFilter, targets);
        if (targets.Count == 0) return;
        foreach (var target in targets)
        {
            if (target.TryGetComponent<HealthComponent>(out var targetHealth))
            {
                targetHealth.TakeDamage(model.Damage);
            }
        }
       Destroy(gameObject);
    }
}
