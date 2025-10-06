using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Subscribed on player's input events and events invoked in attack animations
/// Processes player's movement and invoke events for attacks
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerModel playerModel;
    private Vector2 moveDirection;

    public UnityEvent StartMelee;
    public UnityEvent StartMeleeActive;
    public UnityEvent EndMeleeActive;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerModel = GetComponent<PlayerModel>();
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
}
