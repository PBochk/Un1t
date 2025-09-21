using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerModel playerModel;
    private MeleeWeaponController meleeController;
    private Vector2 moveDirection;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerModel = GetComponent<PlayerModel>();
        meleeController = GetComponentInChildren<MeleeWeaponController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        MovePlayer(moveDirection);
        //if (playerInputActions.Player.MeleeAttack.IsPressed())
        //{
        //    MeleeAttack();
        //}
    }

    private void MovePlayer(Vector2 inputVector)
    {
        rb.MovePosition(rb.position + inputVector * playerModel.MovingSpeed * Time.fixedDeltaTime);
    }

    private void MeleeAttack()
    {
        meleeController.Attack();
    }
}
