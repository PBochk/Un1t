using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ProjectileModelMB))]
public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask solid;
    [SerializeField] private bool canBeUpgraded;
    private ProjectileModel model;
    private Collider2D projectileCollider;
    private ContactFilter2D contactFilter;
    private readonly HashSet<Hitable> hittables = new();
    private Coroutine lifetimeCoroutine;
    private bool toBeDestroyed = false;
    
    public UnityEvent EnemyHit;
    public UnityEvent WallHit;
    public UnityEvent Destroyed;
    
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
        lifetimeCoroutine = StartCoroutine(DestroyProjectile());
    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(model.Lifetime);
        Destroyed?.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (toBeDestroyed) return;
        var targets = new List<Collider2D>();
        Physics2D.OverlapCollider(projectileCollider, contactFilter, targets);
        if (targets.Count == 0) return;
        StartCoroutine(WaitForDestroy());
        foreach (var target in targets)
        {
            if (!target.TryGetComponent<Hitable>(out var hittable)) continue;
            if (!hittables.Add(hittable)) return;
            
            hittable.HitTaken?.Invoke(model.AttackData);
            EnemyHit?.Invoke();
            return;
        }
        WallHit?.Invoke();
    }

    // Temporary solution of delaying destruction for sound to play
    // TODO: rework with animation
    private IEnumerator WaitForDestroy()
    {
        toBeDestroyed = true;
        yield return null;
        StopCoroutine(lifetimeCoroutine);
        projectileCollider.enabled = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        GetComponent<SpriteRenderer>().enabled = false;
        Destroyed?.Invoke();
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
