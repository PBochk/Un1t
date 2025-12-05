using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Sets animation triggers, plays sounds
/// </summary>
// TODO: rename methods according to conventions
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerModelMB))]
[RequireComponent(typeof(PlayerController))]
public class PlayerView : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerMeleeWeaponController meleeController;
    [SerializeField] private PlayerRangeWeaponController rangeController;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip damageTakenSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip meleeAttackSound;
    [SerializeField] private AudioClip rangeAttackSound;
    private PlayerModel playerModel;
    private PlayerController playerController;
    private Animator animator;
    private bool isFacingRight = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        playerController.DirectionChanged.AddListener(OnDirectionChanged);
        playerController.StartDash.AddListener(OnStartDash);
        meleeController.StartMeleeAnimation.AddListener(MeleeAttackAnimationStart);
        rangeController.StartRangeAnimation.AddListener(RangeAnimationStart);
    }

    private void Start()
    {
        playerModel = GetComponent<PlayerModelMB>().PlayerModel;
        // TODO: move subscription in OnEnable after model initialization rework
        playerModel.PlayerDeath += OnDeath;
        playerModel.DamageTaken += OnDamageTaken;
    }

    //private void OnDisable()
    //{
    //    playerModel.PlayerDeath -= OnDeath;
    //}

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
        }
        animator.SetBool("IsRunningForward", moveDirection != Vector2.zero);
    }

    private void OnDirectionChanged(int newDirection)
    {
        animator.SetInteger("Direction", newDirection);
    }

    private void OnStartDash()
    {
        animator.SetTrigger("Dash");
        audioSource.PlayOneShot(dashSound);
    }

    private void MeleeAttackAnimationStart()
    {
        animator.SetTrigger("MeleeAttack");
        audioSource.PlayOneShot(meleeAttackSound);
    }

    private void RangeAnimationStart()
    {
        animator.SetTrigger("RangeAttack");
        audioSource.PlayOneShot(rangeAttackSound);
    }

    private void OnDeath()
    {
        animator.SetTrigger("PlayerDeath");
        audioSource.PlayOneShot(deathSound);
    }

    private void OnDamageTaken()
    {
        damageParticles.Play();
        audioSource.PlayOneShot(damageTakenSound);
    }
}
