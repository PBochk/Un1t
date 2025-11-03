using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerRangeWeaponModelMB))]
public class PlayerRangeWeaponController : MonoBehaviour
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform playerTransform;
    private PlayerRangeWeaponModel model;
    private PlayerRangeWeaponModelMB modelMB;
    private PlayerController playerController;

    public UnityEvent StartRangeAnimation;

    private void Awake()
    {
        modelMB = GetComponent<PlayerRangeWeaponModelMB>();
        playerController = GetComponentInParent<PlayerController>();
        playerController.StartRange.AddListener(StartRange);
        playerController.RangeShot.AddListener(ShootProjectile);
    }

    private void Start()
    {
        model = modelMB.PlayerRangeWeaponModel;
    }

    private void StartRange()
    {
        Debug.Log("Start range animation attempt");
        if (model.Ammo > 0 && modelMB.IsAttackReady)
        {
            Debug.Log("Start range animation");
            StartRangeAnimation?.Invoke();
            StartCoroutine(modelMB.WaitForAttackCooldown());
        }
    }

    private void ShootProjectile()
    {
        Debug.Log("Shoot projectile");
        var shotDirection = (playerController.MousePosition - (Vector2)transform.position).normalized;
        var spawnedProjectile = Instantiate(projectile.gameObject, transform.position, GetShotAngle(shotDirection));
        spawnedProjectile.GetComponent<Rigidbody2D>().AddForce(shotDirection * modelMB.InitialForce);
        model.SpendAmmo();
    }

    private Quaternion GetShotAngle(Vector2 shotDirection)
    {
        var rotationZ = Mathf.Atan2(shotDirection.y, shotDirection.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(playerTransform.localRotation.x,
                                playerTransform.localRotation.y, 
                                rotationZ);
    }
}
