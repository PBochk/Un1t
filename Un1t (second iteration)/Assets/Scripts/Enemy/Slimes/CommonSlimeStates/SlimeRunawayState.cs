using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SlimeRunawayState : EnemyState
{
    
    public UnityEvent jumpStart;
    
    [SerializeField] public float baseMoveTime { get; private set; } = 0.75f;
    
    private WaitForFixedUpdate physicsUpdate = new WaitForFixedUpdate();
    private float moveTimer = 0;
    private Vector2 startPosition;
    private Vector2 direction;
    
    private Rigidbody2D enemyRb;

    protected override void Awake()
    {
        base.Awake();
        enemyRb =  GetComponent<Rigidbody2D>();
    }
    
    //TODO: rewrite logic to invert movement
    public override void EnterState(EnemyTargetComponent target)
    {
        base.EnterState(target);
        var distance = model.Config.BaseMoveSpeed;
        startPosition = enemyRb.position;
        direction =  (enemyRb.position - target.Position).normalized * distance;
        //TODO: Fix Possible DivisionByZeroException 
        StartCoroutine(Jump(baseMoveTime / model.NativeModel.SpeedCoeff));
    }

    //TODO: Maybe make a windup animation
    private IEnumerator Jump(float moveTime)
    {
        while (moveTimer <= moveTime)
        {
            enemyRb.MovePosition(startPosition + direction * moveTimer /  moveTime);
            moveTimer += Time.fixedDeltaTime;
            yield return physicsUpdate;
        }
        moveTimer = 0f;
        ExitState();
    }
}