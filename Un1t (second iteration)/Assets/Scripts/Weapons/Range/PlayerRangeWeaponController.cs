using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerRangeWeaponModelMB))]
public class PlayerRangeWeaponController : MonoBehaviour
{
    [SerializeField] private ProjectileController projectile;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float initialForce;
    private PlayerRangeWeaponModel model;
    private PlayerRangeWeaponModelMB modelMB;
    private PlayerController playerController;

    private Vector2 targetPosition;

    public UnityEvent StartRangeAnimation;

    private void Awake()
    {
        modelMB = GetComponent<PlayerRangeWeaponModelMB>();
        playerController = GetComponentInParent<PlayerController>();
        playerController.StartRange.AddListener(OnStartRange);
        playerController.RangeShot.AddListener(ShootProjectile);
    }

    private void Start()
    {
        model = modelMB.RangeWeaponModel;
    }

    private void OnStartRange()
    {
        if (model.Ammo > 0 && modelMB.IsAttackReady)
        {
            StartRangeAnimation?.Invoke();
            StartCoroutine(modelMB.WaitForAttackCooldown());
            targetPosition = playerController.MousePosition;
        }
    }

    private void ShootProjectile()
    {
        var shotDirection = (targetPosition - (Vector2)transform.position).normalized;
        var spawnedProjectile = Instantiate(projectile.gameObject, transform.position, Quaternion.FromToRotation(playerTransform.position, shotDirection));
        spawnedProjectile.GetComponent<ProjectileController>().Initialize(model.ProjectileModel); // TODO: rework
        spawnedProjectile.GetComponent<Rigidbody2D>().AddForce(shotDirection * initialForce);
        model.SpendAmmo();
    }
}
