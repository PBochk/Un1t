using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private EnemyDummyTarget dummyTargetPrefab;
    private EnemyDummyTarget dummyTarget;
    private IEnemyTarget target;
    private EnemyTargetComponent targetComponent;

    public UnityEvent<EnemyController> EnemySpawned;
    
    //TODO: Methods for enemy creation, and in initialization this should be on creation step
    //TODO: Subscribe experience component on enemy death
    private void Awake()
    {
        dummyTarget = Instantiate(dummyTargetPrefab);
    }

    public void SetTarget(EnemyTargetComponent targetComponent)
    {
        this.targetComponent = targetComponent;
    }

    public void CreateEnemy()
    {
        var enemy = Instantiate(enemyPrefab);
        enemy.SetTarget(targetComponent);
        //var model = enemy.GetComponent<EnemyModelMB>();
    }
}
