using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ProjectileModelMB))]
public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask solid;
    [SerializeField] private bool canBeUpgraded;
    private ProjectileModel model;
    private Collider2D projectileCollider;
    private ContactFilter2D contactFilter;

    private void Awake()
    {
        projectileCollider = GetComponent<Collider2D>();
        contactFilter.SetLayerMask(solid);
    }

    /// <summary>
    /// Set default model if there is no need to change model in upgrades
    /// </summary>
    private void Start()
    {
        if (canBeUpgraded) return;
        Initialize(GetComponent<ProjectileModelMB>().ProjectileModel); 
    }

    public void Initialize(ProjectileModel model)
    {
        this.model = model;
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
            if (target.TryGetComponent<Hitable>(out var hittable))
            {
                hittable.HitTaken?.Invoke(model.AttackData);
            }
        }
       Destroy(gameObject);
    }
}
