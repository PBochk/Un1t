using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    private float damage;
    private float lifetime;
    private Collider2D projectileCollider;
    private ContactFilter2D contactFilter;

    private void Awake()
    {
        projectileCollider = GetComponent<Collider2D>();
    }

    public void Initialize(float damage, float lifetime, LayerMask solid)
    {
        this.damage = damage;
        this.lifetime = lifetime;
        contactFilter.SetLayerMask(solid);
        StartCoroutine(DestroyProjectile());
    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var targets = new List<Collider2D>();
        Physics2D.OverlapCollider(projectileCollider, contactFilter, targets);
        if (targets.Count == 0) return;
        foreach (var target in targets)
        {
            var targetHealth = target.GetComponent<HealthComponent>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
