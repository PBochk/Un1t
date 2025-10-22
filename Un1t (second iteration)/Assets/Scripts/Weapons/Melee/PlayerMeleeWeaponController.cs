using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An implementation of melee weapon controller for player to use.
/// </summary>

[RequireComponent(typeof(PlayerMeleeWeaponModelMB))]
public class PlayerMeleeWeaponController : MeleeWeaponController
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private PlayerController playerController;
    public UnityEvent StartMeleeAnimation;

    /// <summary>
    /// Overrides abstract model on player's implementation
    /// and subscribes base class methods on player's events
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        modelMB = GetComponent<PlayerMeleeWeaponModelMB>();
        playerController = GetComponentInParent<PlayerController>();
        playerController.StartMelee.AddListener(StartMelee);
        playerController.StartMeleeActive.AddListener(StartMeleeActive);
        playerController.EndMeleeActive.AddListener(EndMeleeActive);
    }

    /// <summary>
    /// When attack is ready starts cooldown in model
    /// </summary>
    protected override void StartMelee()
    {
        base.StartMelee();
        if (modelMB.IsAttackReady)
        {
            StartMeleeAnimation?.Invoke();
            StartCoroutine(((PlayerMeleeWeaponModelMB)modelMB).WaitForAttackCooldown());
        }
    }
    public void SetRendererActive(bool isActive)
    {
        spriteRenderer.gameObject.SetActive(isActive);
    }
}
