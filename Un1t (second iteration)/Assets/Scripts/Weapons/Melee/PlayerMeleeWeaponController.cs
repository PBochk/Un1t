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
    public UnityEvent StartPickaxeAnimation;

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
            if(playerController.EquippedTool == PlayerTools.Melee)
            {
                StartMeleeAnimation?.Invoke();
            }
            else if (playerController.EquippedTool == PlayerTools.Pickaxe)
            {
                StartPickaxeAnimation?.Invoke();
            }
            StartCoroutine(((PlayerMeleeWeaponModelMB)modelMB).WaitForAttackCooldown());
        }
    }

    /// <summary>
    /// Temporary solution for displaying weapon's change
    /// </summary>
    // TODO: rework it in view when there are animations
    public void SetRendererActive(bool isActive)
    {
        spriteRenderer.gameObject.SetActive(isActive);
    }
}
