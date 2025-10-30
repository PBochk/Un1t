using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Sets animation triggers, plays sounds
/// There will be player related UI too
/// </summary>

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerModelMB))]
[RequireComponent(typeof(PlayerController))]
// TODO: figure out how to require component in children
public class PlayerView : MonoBehaviour
{
    // TODO: move UI in separate class 
    [SerializeField] private Transform playerTransform;
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private PlayerMeleeWeaponController meleeController;
    [SerializeField] private PlayerMeleeWeaponController pickaxeController;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text xpText;
    private PlayerModelMB playerModelMB;
    private PlayerModel playerModel;
    private PlayerController playerController;
    private Animator animator;
    private bool isFacingRight = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerModelMB = GetComponent<PlayerModelMB>();
        playerController = GetComponent<PlayerController>();
        meleeController.StartMeleeAnimation.AddListener(MeleeAttackAnimationStart);
        playerController.StartMelee.AddListener(OnMelee);
        pickaxeController.StartPickaxeAnimation.AddListener(PickaxeAnimationStart);
    }

    private void Start()
    {
        playerModel = playerModelMB.PlayerModel;
        playerModel.HealthChanged += OnHealthChanged; //Should be in OnEnable
        playerModel.ExperienceChanged += OnExperienceChanged; //Should be in OnEnable
        Initialize();
    }

    private void OnDisable()
    {
        playerModel.HealthChanged -= OnHealthChanged;
        playerModel.ExperienceChanged -= OnExperienceChanged;
    }

    private void Initialize()
    {
        OnHealthChanged();
        OnExperienceChanged();
    }

    public void OnMove(InputValue value)
    {
        var moveDirection = value.Get<Vector2>();
        if (moveDirection.x < 0 && isFacingRight 
            || moveDirection.x > 0 && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            playerTransform.localScale = new Vector3(playerTransform.localScale.x * (-1),
                                                     playerTransform.localScale.y,
                                                     playerTransform.localScale.z);

            // Line below changes player's facing direction more correctly, but breaks camera
            // playerTransform.RotateAround(playerTransform.position, Vector2.up, 180);
        }
        animator.SetBool("IsRunningForward", moveDirection.x != 0);
    }
    private void MeleeAttackAnimationStart()
    {
        animator.SetTrigger("MeleeAttack");
    }

    private void OnMelee()
    {
        attackSound.Play();
    }

    private void PickaxeAnimationStart()
    {
        animator.SetTrigger("PickaxeAttack");
    }

    private void OnHealthChanged()
    {
        hpText.text = playerModel.CurrentHealth + " / " + playerModel.MaxHealth;
    }
    private void OnExperienceChanged()
    {
        xpText.text =  playerModel.CurrentXP + " / " + playerModel.NextLevelXP;
    }

}
