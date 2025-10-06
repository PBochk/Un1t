using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IEnemyTarget
{
    private Rigidbody2D rb;
    private PlayerModel playerModel;
    private MeleeWeaponController meleeController;
    private Vector2 moveDirection;
    public Vector2 Position => rb.position;

    public UnityEvent onAttack;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerModel = GetComponent<PlayerModel>();
        meleeController = GetComponentInChildren<MeleeWeaponController>();
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

    public void StartMeleeAttack(InputAction.CallbackContext context) => meleeController.Attack();

    public void OnMeleeAttack()
    {
        meleeController.OnAttack();
        onAttack?.Invoke();
    }


}
