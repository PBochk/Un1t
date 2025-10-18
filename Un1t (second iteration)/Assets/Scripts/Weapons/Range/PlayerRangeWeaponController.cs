using UnityEngine;

[RequireComponent(typeof(PlayerRangeWeaponModelMB))]
public class PlayerRangeWeaponController : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform playerTransform;
    private PlayerRangeWeaponModel model;
    private PlayerRangeWeaponModelMB modelMB;
    private PlayerController playerController;

    private void Awake()
    {
        modelMB = GetComponent<PlayerRangeWeaponModelMB>();
        playerController = GetComponentInParent<PlayerController>();
        playerController.StartRange.AddListener(StartRange);
    }

    private void Start()
    {
        model = modelMB.playerRangeWeaponModel;
    }

    private void StartRange()
    {
        if (model.Ammo > 0 && modelMB.IsAttackReady)
        {
            var shotDirection = (playerController.MousePosition - (Vector2)transform.position).normalized;
            var spawnedProjectile = Instantiate(projectile.gameObject, transform.position, GetShotAngle(shotDirection));
            spawnedProjectile.GetComponent<Rigidbody2D>().AddForce(shotDirection * modelMB.InitialForce);
            StartCoroutine(modelMB.WaitForAttackCooldown());
            model.SpendAmmo();
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
