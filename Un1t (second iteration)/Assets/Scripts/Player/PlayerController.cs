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

    private Vector2 moveDirection;
    private Vector2 dashDirection = Vector2.right;
    private bool isDashing;
    private bool canDash = true;
    public Vector2 MousePosition { get; private set; }

    public UnityEvent StartMelee;
    public UnityEvent StartMeleeActive;
    public UnityEvent EndMeleeActive;
    public UnityEvent StartRange;
    public UnityEvent RangeShot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        GetComponent<Hitable>().HitTaken.AddListener(OnHitTaken);
    }

    private void Start()
    {
        playerModel = GetComponent<PlayerModelMB>().PlayerModel;
        // TODO: move subscription in OnEnable after model initialization rework
        playerModel.PlayerRestrained += SetInputEnabled;
    }

    private void OnDisable()
    {
        playerModel.PlayerRestrained -= SetInputEnabled;
    }
    public void SetInputEnabled()
    {
        playerInput.enabled = !playerModel.IsRestrained;
    }

    public void OnMouseMove(InputValue value)
    {
        var screenPosition = value.Get<Vector2>();
        MousePosition = Camera.main.ScreenToWorldPoint(screenPosition);
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            MovePlayer(dashDirection, playerModel.DashSpeed);
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
        if (moveDirection != Vector2.zero && !isDashing)
        {
            dashDirection = moveDirection;
        }
    }

    public void OnDash()
    {
        if (canDash)
        {
            StartCoroutine(StartDash());
            StartCoroutine(WaitForDashCooldown());
        }
    }

    private IEnumerator StartDash()
    {
        isDashing = true;
        yield return new WaitForSeconds(playerModel.DashDuration);
        isDashing = false;
    }

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
    }

}
