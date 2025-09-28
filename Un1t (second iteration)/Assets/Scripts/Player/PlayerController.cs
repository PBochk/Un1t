using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerModel playerModel;
    private Vector2 moveDirection;

    public UnityEvent onMeleeAttackStart;
    public UnityEvent onMeleeAttack;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerModel = GetComponent<PlayerModel>();
    }

    private void FixedUpdate()
    {
        MovePlayer(moveDirection);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    private void MovePlayer(Vector2 inputVector)
    {
        rb.MovePosition(rb.position + inputVector * playerModel.MovingSpeed * Time.fixedDeltaTime);
    }

    public void StartMeleeAttack(InputAction.CallbackContext context)
    {
        onMeleeAttackStart?.Invoke();
    }

    public void OnMeleeAttack()
    {
        onMeleeAttack?.Invoke();
    }
}
