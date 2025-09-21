using UnityEngine;

public class MeeleeEnemyController : EnemyController
{
    [SerializeField] private float Speed;
    protected override void Awake()
    {
        currentState = new MeeeleeChaseState();
        model = new EnemyModel(0, 0, 1.5f);
        base.Awake();
    }
}