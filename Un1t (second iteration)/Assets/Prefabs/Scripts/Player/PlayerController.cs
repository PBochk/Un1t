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

public class PlayerController : MonoBehaviour, IEnemyTarget
{
    private Rigidbody2D rb;
    private PlayerInput playerInput; 
    private PlayerModel playerModel;
    [SerializeField] private PlayerMeleeWeaponController meleeController;
    [SerializeField] private PlayerMeleeWeaponController pickaxeController;
    [SerializeField] private PlayerRangeWeaponController rangeController;

    private Vector2 moveDirection;
    public Vector2 Position => rb.position;
    public Vector2 MousePosition { get; private set; }

    private PlayerTools lastTool = PlayerTools.None;
    private PlayerTools equippedTool = PlayerTools.None;
    public PlayerTools EquippedTool => equippedTool;

    public UnityEvent StartMelee;
    public UnityEvent StartMeleeActive;
    public UnityEvent EndMeleeActive;
    public UnityEvent StartRange;
    //public UnityEvent<Vector2> MouseMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        GetComponent<Hitable>().HitTaken.AddListener(OnHitTaken);
        //meleeController = GetComponentInChildren<PlayerMeleeWeaponController>();
        //rangeController = GetComponentInChildren<PlayerRangeWeaponController>();
    }

    private void Start()
    {
        playerModel = GetComponent<PlayerModelMB>().PlayerModel;
    }

    private void FixedUpdate()
    {
        MovePlayer(moveDirection);
    }

    public void SetInputEnabled(bool isEnabled) => playerInput.enabled = isEnabled;

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

    public void OnAttack()
    {
        if (equippedTool == PlayerTools.Melee || equippedTool == PlayerTools.Pickaxe)
        {
            StartMelee?.Invoke();
        }
        else if (equippedTool == PlayerTools.Range)
        {
            StartRange?.Invoke();
        }
    }

    public void OnMeleeActiveStart()
    {
        StartMeleeActive?.Invoke();
    }
    public void OnMeleeActiveEnd()
    {
        EndMeleeActive?.Invoke();
    }

    public void OnMouseMove(InputValue value)
    {
        var screenPosition = value.Get<Vector2>();
        MousePosition = Camera.main.ScreenToWorldPoint(screenPosition);
    }

    //Player starts with no weapon equipped
    //Equipping keys:
    //1 - melee
    //2 - range
    //q - previous weapon

    public void OnEquipLastTool()
    {
        (lastTool, equippedTool) = (equippedTool, lastTool);
        ChangeTool();
    }

    public void OnEquipMelee()
    {
        if (playerModel.AvailableTools.Contains(PlayerTools.Melee))
        {
            lastTool = equippedTool;
            equippedTool = PlayerTools.Melee;
            ChangeTool();
        }
    }

    public void OnEquipRange()
    {
        if (playerModel.AvailableTools.Contains(PlayerTools.Range))
        {
            lastTool = equippedTool;
            equippedTool = PlayerTools.Range;
            ChangeTool();
        }
    }

    public void OnEquipPickaxe()
    {
        if (playerModel.AvailableTools.Contains(PlayerTools.Pickaxe))
        {
            lastTool = equippedTool;
            equippedTool = PlayerTools.Pickaxe;
            ChangeTool();
        }
    }

    /// <summary>
    /// Temporary solution for displaying weapon's change
    /// </summary>
    private void ChangeTool()
    {
        if (meleeController)
        {
            meleeController.SetRendererActive(equippedTool == PlayerTools.Melee);
        }
        if (pickaxeController)
        {
            pickaxeController.SetRendererActive(equippedTool == PlayerTools.Pickaxe);
            
        }
    }
}
