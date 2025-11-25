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
        if (model.Ammo > 0 && modelMB.IsAttackReady)
        {
            StartRangeAnimation?.Invoke();
            StartCoroutine(modelMB.WaitForAttackCooldown());
        }
    }

    private void ShootProjectile()
    {
        var shotDirection = (playerController.MousePosition - (Vector2)transform.position).normalized;
        var spawnedProjectile = Instantiate(projectile.gameObject, transform.position, Quaternion.FromToRotation(playerTransform.position, shotDirection));
        spawnedProjectile.GetComponent<ProjectileController>().Initialize(model.ProjectileModel); // TODO: rework
        spawnedProjectile.GetComponent<Rigidbody2D>().AddForce(shotDirection * initialForce);
        model.SpendAmmo();
    }
}
