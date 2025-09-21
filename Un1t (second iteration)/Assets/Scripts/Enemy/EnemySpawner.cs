using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private EnemyDummyTarget dummyTargetPrefab;
    private EnemyDummyTarget dummyTarget;
    
    //TODO: Methods for enemy creation, and in initialization this should be on creation step
    private void Awake()
    {
        dummyTarget = Instantiate(dummyTargetPrefab);
        var enemy = Instantiate(enemyPrefab);
        enemy.SetTarget(dummyTarget);
    }

    private void SetTarget(IEnemyTarget target)
    {
        
    }
}
