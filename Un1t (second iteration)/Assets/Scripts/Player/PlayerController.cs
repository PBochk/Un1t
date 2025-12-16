using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Subscribed on player's input events and events invoked in attack animations
/// Processes player's movement and invoke events for attacks
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerModelMB))]

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private PlayerModel playerModel;
    private PlayerHitable playerHitable;

    public Vector2 MousePosition { get; private set; }

    private Vector2 moveDirection;
    private Vector2 lastMoveDirection = Vector2.right;
    private bool isDashing;
    private bool canDash = true;
    private List<int> layersToExcludeOnDash;

    private const float PUSH_TIME = 0.3f;
    private float pushSpeed;
    private Vector2 pushDirection;
    private bool isPushed = false;

    public UnityEvent<int> DirectionChanged;
    public UnityEvent StartMelee;
    public UnityEvent StartMeleeActive;
    public UnityEvent EndMeleeActive;
    public UnityEvent StartRange;
    public UnityEvent RangeShot;
    public UnityEvent StartDash;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerHitable = GetComponent<Hitable>() as PlayerHitable;
        layersToExcludeOnDash = new() { LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Breakable") };
        playerHitable.HitTaken.AddListener(OnHitTaken);
    }

    private void Start()
    {
        playerModel = GetComponent<PlayerModelMB>().PlayerModel;
        // TODO: move subscription to OnEnable after initialization rework
        playerModel.PlayerDeath += () => SetPlayerRestrained(true);
    }

    private void OnDisable()
    {
        playerModel.PlayerDeath -= () => SetPlayerRestrained(true);
    }

    public void SetPlayerRestrained(bool isRestrained)
    {
        playerInput.enabled = !isRestrained;
    }

    /// <summary>
    /// Allows using <see cref="SetPlayerRestrained">SetPlayerRestrained(true)</see> from animator
    /// </summary>
    public void EnablePlayerRestrain() => SetPlayerRestrained(true);
    /// <summary>
    /// Allows using <see cref="SetPlayerRestrained">SetPlayerRestrained(false)</see> from animator
    /// </summary>
    public void DisablePlayerRestrain() => SetPlayerRestrained(false);

    public void OnMouseMove(InputValue value)
    {
        var screenPosition = value.Get<Vector2>();
        MousePosition = Camera.main.ScreenToWorldPoint(screenPosition);
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            MovePlayer(lastMoveDirection, playerModel.DashSpeed);
        }
        else if (isPushed)
        {
            MovePlayer(pushDirection, pushSpeed);
        }
        else
        {
            MovePlayer(moveDirection, playerModel.MovingSpeed);
        }
    }

    private void MovePlayer(Vector2 direction, float speed)
    {
        rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * direction);
    }

    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(lastMoveDirection.x))
        {
            DirectionChanged?.Invoke(0);
        }
        else if (moveDirection.y != 0 && Mathf.Abs(moveDirection.y) >= Mathf.Abs(lastMoveDirection.y))
        {
            DirectionChanged?.Invoke((int)Mathf.Sign(moveDirection.y));
        }
        if (moveDirection != Vector2.zero && !isDashing)
        {
            lastMoveDirection = moveDirection;
        }
    }

    public void OnDash()
    {
        if (canDash)
        {
            StartDash?.Invoke();
            StartCoroutine(WaitForDashDuration());
            playerHitable.SetInvulForSeconds(playerModel.DashDuration);
            StartCoroutine(WaitForDashCooldown());
        }
    }

    /// <summary>
    /// Set isDashing true for dash duration
    /// </summary>
    /// <remarks>
    /// Set player restrained because player shouldn't be able to attack while dashing
    /// </remarks>
    private IEnumerator WaitForDashDuration()
    {
        isDashing = true;
        SetPlayerRestrained(true);
        // i took this eldritch horror directly from unity documentation
        foreach (var layer in layersToExcludeOnDash)
        {
            rb.excludeLayers |= (1 << layer);
        }
        yield return new WaitForSeconds(playerModel.DashDuration);
        foreach (var layer in layersToExcludeOnDash)
        {
            rb.excludeLayers &= ~(1 << layer);
        }
        SetPlayerRestrained(false);
        isDashing = false;
    }

    /// <summary>
    /// Set the ability to dash on cooldown
    /// </summary>
    public IEnumerator WaitForDashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(playerModel.DashCooldown);
        canDash = true;
    }

    public void OnMeleeAttack()
    {
        StartMelee?.Invoke();
    }

    public void OnRangeAttack()
    {
        StartRange?.Invoke();
    }

    public void OnMeleeActiveStart()
    {
        StartMeleeActive?.Invoke();
    }
    public void OnMeleeActiveEnd()
    {
        EndMeleeActive?.Invoke();
    }

    private void OnRangeShot()
    {
        RangeShot?.Invoke();
    }

    public void OnHitTaken(AttackData attackData)
    {
        playerModel.TakeDamage(attackData.Damage);
        playerModel.DecreaseXP(attackData.XPDamage);
        Debug.Log("Player took damage: " + attackData.Damage + " current hp: " + playerModel.CurrentHealth);
        if (attackData.AttackerTransform != null && !isPushed)
        {
            pushDirection = (transform.position - attackData.AttackerTransform.position).normalized;
            pushSpeed = attackData.PushSpeed;
            StartCoroutine(WaitForPush());
        }
    }

    private IEnumerator WaitForPush()
    {
        isPushed = true;
        yield return new WaitForSeconds(PUSH_TIME);
        isPushed = false;
    }

    public void OnLevelUp()
    {
        if (playerModel.IsLevelUpAvailable)
        {
            playerModel.LevelUp();
        }
    }

    public void OnHeal()
    {
        playerModel.TakeHeal(playerModel.MaxHealth, playerModel.HealCostInXP);
    }
}
