using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private Rigidbody2D rb;
    private PlayerModel playerModel;
    private MeleeWeaponController meleeController;
    void Awake()
    {
        playerInputActions = new();
        playerInputActions.Enable();
        rb = GetComponent<Rigidbody2D>();
        playerModel = GetComponent<PlayerModel>();
        meleeController = GetComponentInChildren<MeleeWeaponController>();
    }

    private Vector2 GetMovementVector() => playerInputActions.Player.Move.ReadValue<Vector2>();
    
    private void FixedUpdate()
    {
        Vector2 inputVector = GetMovementVector().normalized;
        rb.MovePosition(rb.position + inputVector * playerModel.MovingSpeed * Time.fixedDeltaTime);
        if (playerInputActions.Player.MeleeAttack.IsPressed())
        {
            MeleeAttack();
        }

    }

    private void MeleeAttack()
    {
        meleeController.Attack();
    }
}
