using UnityEngine.Events;

public class PlayerMeleeWeaponController : MeleeWeaponController
{
    private PlayerController playerController;
    public UnityEvent onMeleeAttackStart;

    protected override void Awake()
    {
        model = GetComponent<PlayerMeleeWeaponModel>();
        playerController = GetComponentInParent<PlayerController>();
        playerController.onMeleeAttackStart.AddListener(StartMeleeAttack);
        playerController.onMeleeAttack.AddListener(OnMeleeAttack);
        base.Awake();
    }

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
