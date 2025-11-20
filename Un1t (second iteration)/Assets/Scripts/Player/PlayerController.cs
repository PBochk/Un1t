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
    private PlayerMeleeWeaponController meleeController;
    private PlayerRangeWeaponController rangeController;

    private Vector2 moveDirection;
    public Vector2 Position => rb.position;
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
        meleeController = GetComponentInChildren<PlayerMeleeWeaponController>();
        rangeController = GetComponentInChildren<PlayerRangeWeaponController>();
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

    private void FixedUpdate()
    {
        MovePlayer(moveDirection);
    }

    public void SetInputEnabled()
    {
        playerInput.enabled = !playerModel.IsRestrained;
    }

    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }

    private void MovePlayer(Vector2 inputVector)
    {
        rb.MovePosition(rb.position + playerModel.MovingSpeed * Time.fixedDeltaTime * inputVector);
    }

    public void OnHitTaken(AttackData attackData)
    {
        playerModel.TakeDamage(attackData.Damage);
        Debug.Log("Player took damage: " + attackData.Damage + " current hp: " + playerModel.CurrentHealth);
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

    public void OnMouseMove(InputValue value)
    {
        var screenPosition = value.Get<Vector2>();
        MousePosition = Camera.main.ScreenToWorldPoint(screenPosition);
    }
}
