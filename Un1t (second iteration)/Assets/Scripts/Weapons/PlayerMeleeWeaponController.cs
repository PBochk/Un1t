using UnityEngine.Events;

/// <summary>
/// An implementation of melee weapon controller for player to use.
/// </summary>
public class PlayerMeleeWeaponController : MeleeWeaponController
{
    private PlayerController playerController;

    /// <summary>
    /// Overrides abstract model on player's implementation
    /// and subscribes base class methods on player's events
    /// </summary>
    protected override void Awake()
    {
        model = GetComponent<PlayerMeleeWeaponModel>();
        playerController = GetComponentInParent<PlayerController>();
        playerController.StartMelee.AddListener(StartMelee);
        playerController.StartMeleeActive.AddListener(StartMeleeActive);
        playerController.EndMeleeActive.AddListener(EndMeleeActive);
        base.Awake();
    }

    /// <summary>
    /// When attack is ready starts cooldown in model
    /// </summary>
    protected override void StartMelee()
    {
        base.StartMelee();
        if (model.IsAttackReady)
        {
            StartCoroutine(((PlayerMeleeWeaponModel)model).WaitForAttackCooldown());
        }
    }
}
