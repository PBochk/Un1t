using UnityEngine;

public class SingleShotWeapon : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    [Tooltip("Used to add force")]
    [SerializeField] private float ProjectileSpeed = 3f;

    public void Shot(EnemyTargetComponent target)
    {
        var targetPosition = target.Position;
        var direction = targetPosition - (Vector2)transform.position;
        var rot = Quaternion.FromToRotation(Vector3.right, targetPosition);
        var projInstance = Instantiate(projectile, transform.position, rot);
        projInstance.GetComponent<Rigidbody2D>().AddForce(direction.normalized * ProjectileSpeed, ForceMode2D.Impulse);
    }
}