using TMPro;
using UnityEngine;

/// <summary>
/// Sets animation triggers, plays sounds
/// There will be player related UI too
/// </summary>
public class PlayerView : MonoBehaviour
{
    [SerializeField] private AudioSource attackSound;
    private PlayerController controller;
    private PlayerMeleeWeaponController weaponController;
    private Animator animator;
    public void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        weaponController = GetComponentInChildren<PlayerMeleeWeaponController>();
        weaponController.StartMeleeAnimation.AddListener(MeleeAttackAnimationStart);
        controller.StartMelee.AddListener(OnMelee);
    }

<<<<<<< Updated upstream
=======
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

            // Line above changes player's facing direction more correctly, but breaks camera
            // playerTransform.RotateAround(playerTransform.position, Vector2.up, 180);
        }
        animator.SetBool("IsRunningForward", moveDirection.x != 0);
    }

>>>>>>> Stashed changes
    private void MeleeAttackAnimationStart()
    {
        animator.SetTrigger("MeleeAttack");
    }

    private void OnMelee()
    {
        attackSound.Play();
    }
}
