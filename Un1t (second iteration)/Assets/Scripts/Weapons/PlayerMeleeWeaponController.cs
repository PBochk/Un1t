using UnityEngine.Events;

/// <summary>
/// An implementation of melee weapon controller for player to use.
/// </summary>
public class PlayerMeleeWeaponController : MeleeWeaponController
{
    private PlayerController playerController;
    public UnityEvent onMeleeAttackStart;

    /// <summary>
    /// Overrides abstract model on player's implementation
    /// and subscribes base class methods on player's events
    /// </summary>
    protected override void Awake()
    {
        model = GetComponent<PlayerMeleeWeaponModel>();
        playerController = GetComponentInParent<PlayerController>();
        playerController.onMeleeAttackStart.AddListener(StartMeleeAttack);
        playerController.onMeleeAttack.AddListener(OnMeleeAttack);
        base.Awake();
    }

    /// <summary>
    /// When attack is ready starts cooldown in model
    /// </summary>
    protected override void StartMeleeAttack()
    {
        base.StartMeleeAttack();
        if (model.IsAttackReady)
        {
            onMeleeAttackStart?.Invoke();
            StartCoroutine(((PlayerMeleeWeaponModel)model).WaitForAttackCooldown());
        }
    }
}
