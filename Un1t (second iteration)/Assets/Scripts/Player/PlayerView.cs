using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Sets animation triggers, plays sounds
/// </summary>

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerModelMB))]
[RequireComponent(typeof(PlayerController))]
public class PlayerView : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private PlayerMeleeWeaponController meleeController;
    [SerializeField] private PlayerMeleeWeaponController pickaxeController;
    private PlayerController playerController;
    private Animator animator;
    private bool isFacingRight = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        meleeController.StartMeleeAnimation.AddListener(MeleeAttackAnimationStart);
        playerController.StartMelee.AddListener(OnMelee);
        pickaxeController.StartPickaxeAnimation.AddListener(PickaxeAnimationStart);
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
}
