using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerRangeWeaponController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
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
            var projectile = Instantiate(projectilePrefab, transform.position, GetShotAngle(shotDirection));
            projectile.GetComponent<Rigidbody2D>().AddForce(shotDirection * model.InitialForce);
            projectile.GetComponent<Projectile>().Initialize(model.Damage, model.Lifetime, model.Solid);
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
