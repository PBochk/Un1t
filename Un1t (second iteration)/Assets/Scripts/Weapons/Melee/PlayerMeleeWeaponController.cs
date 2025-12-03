using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An implementation of melee weapon controller for player to use.
/// </summary>

[RequireComponent(typeof(PlayerMeleeWeaponModelMB))]
public class PlayerMeleeWeaponController : MeleeWeaponController
{
    private PlayerModel playerModel;
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
    
    protected override void Start()
    {
        base.Start();
        playerModel = GetComponentInParent<PlayerModelMB>().PlayerModel;
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

    protected override void EndMeleeActive()
    {
        if (damagedTargets.Count > 0)
        {
            playerModel.HealByHit();
        }
        base.EndMeleeActive();
    }
}
