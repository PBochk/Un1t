using UnityEngine;

public class MeleeEnemyController : EnemyController
{
    [SerializeField] private float Speed;
    protected override void Awake()
    {
        currentState = new MeleeChaseState();
        model = new EnemyModel(0, 0, 1.5f, 1);
        base.Awake();
    }
}