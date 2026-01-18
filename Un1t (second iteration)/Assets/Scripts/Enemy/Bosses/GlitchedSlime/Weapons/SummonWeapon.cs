using UnityEngine;

public class SummonWeapon : MonoBehaviour
{
    
    [SerializeField] private ProjectileController projectile;
    [SerializeField] private float ProjectileSpeed = 3f;
    [SerializeField] private EnemyController SummonablePrefab;

    public void Shot(EnemyTargetComponent target)
    {
        var targetPosition = target.Position;
        var direction = targetPosition - (Vector2)transform.position;
        var rot = Quaternion.FromToRotation(Vector3.right, direction);
        var projInstance = Instantiate(projectile, transform.position, rot);
        projInstance.GetComponent<Rigidbody2D>().AddForce(direction.normalized * ProjectileSpeed, ForceMode2D.Impulse);
        
        projInstance.WallHit.AddListener(() =>
        {
            Summon(target, projInstance.transform.position);
        });
        
        projInstance.EnemyHit.AddListener(() =>
        {
            Summon(target, projInstance.transform.position);
        });
    }

    private void Summon(EnemyTargetComponent target, Vector3 position)
    {
        var summon = Instantiate(SummonablePrefab, position, Quaternion.identity);
        summon.SetTarget(target);
    }
}