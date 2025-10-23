using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Sets animation triggers, plays sounds
/// There will be player related UI too
/// </summary>

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerController))]
// TODO: figure out how to require component in children
public class PlayerView : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private AudioSource attackSound;
    private PlayerController controller;
    [SerializeField] private PlayerMeleeWeaponController meleeController;
    [SerializeField] private PlayerMeleeWeaponController pickaxeController;
    private Animator animator;
    private bool isFacingRight = true;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        //meleeController = GetComponentInChildren<PlayerMeleeWeaponController>();
        meleeController.StartMeleeAnimation.AddListener(MeleeAttackAnimationStart);
        controller.StartMelee.AddListener(OnMelee);
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
