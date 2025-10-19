using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Subscribed on player's input events and events invoked in attack animations
/// Processes player's movement and invoke events for attacks
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerModelMB))]

public class PlayerController : MonoBehaviour, IEnemyTarget
{
    private Rigidbody2D rb;
    private PlayerModel playerModel;
    private Vector2 moveDirection;
    public Vector2 Position => rb.position;

    public Vector2 MousePosition { get; private set; }

    public UnityEvent StartMelee;
    public UnityEvent StartMeleeActive;
    public UnityEvent EndMeleeActive;
    public UnityEvent StartRange;
    //public UnityEvent<Vector2> MouseMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerModel = GetComponent<PlayerModelMB>().PlayerModel;
        GetComponent<Hitable>().HitTaken.AddListener(OnHitTaken);
    }

    private void FixedUpdate()
    {
        MovePlayer(moveDirection);
    }

    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }

    private void MovePlayer(Vector2 inputVector)
    {
        rb.MovePosition(rb.position + inputVector * playerModel.MovingSpeed * Time.fixedDeltaTime);
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

    public void OnMeleeActiveStart()
    {
        StartMeleeActive?.Invoke();
    }
    public void OnMeleeActiveEnd()
    {
        EndMeleeActive?.Invoke();
    }

    public void OnRangeAttack()
    {
        StartRange?.Invoke();
    }

    public void OnMouseMove(InputValue value)
    {
        var screenPosition = value.Get<Vector2>();
        MousePosition = Camera.main.ScreenToWorldPoint(screenPosition);
    }

}
