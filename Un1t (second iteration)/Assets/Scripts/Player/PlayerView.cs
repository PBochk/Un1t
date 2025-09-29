using TMPro;
using UnityEngine;

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
        controller.onMeleeAttack.AddListener(OnMelee);
        weaponController = GetComponentInChildren<PlayerMeleeWeaponController>();
        weaponController.onMeleeAttackStart.AddListener(MeleeAttackAnimationStart);
    }

    private void MeleeAttackAnimationStart()
    {
        animator.SetTrigger("MeleeAttack");
    }

    private void OnMelee()
    {
        attackSound.Play();
    }
}
