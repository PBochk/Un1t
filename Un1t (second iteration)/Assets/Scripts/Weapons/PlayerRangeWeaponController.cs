using UnityEngine;

[RequireComponent(typeof(PlayerRangeWeaponModel))]
public class PlayerRangeWeaponController : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform playerTransform;
    private PlayerRangeWeaponModel model;
    private PlayerController playerController;

    private void Awake()
    {
        model = GetComponent<PlayerRangeWeaponModel>();
        playerController = GetComponentInParent<PlayerController>();
        playerController.StartRange.AddListener(StartRange);
    }

    private void StartRange()
    {
        if (model.IsAttackReady)
        {
            var shotDirection = (playerController.MousePosition - (Vector2)transform.position).normalized;
            // TODO: serialize Projectile component instead of GameObject
            var spawnedProjectile = Instantiate(projectile.gameObject, transform.position, GetShotAngle(shotDirection));
            spawnedProjectile.GetComponent<Rigidbody2D>().AddForce(shotDirection * model.InitialForce);
            //projectile.Initialize(model.Damage, model.Lifetime, model.Solid); // I'm not sure if this should be in separate projectile model
        }
    }

    private Quaternion GetShotAngle(Vector2 shotDirection)
    {
        var rotationZ = Mathf.Atan2(shotDirection.y, shotDirection.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(playerTransform.localRotation.x,
                                playerTransform.localRotation.y, 
                                rotationZ);
    }
}
