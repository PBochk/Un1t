using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "EnemyConfig/EnemyAttack")]
public class EnemyAtackConfig : ScriptableObject
{
    [SerializeField] private AttackData attackData;
    [SerializeField] private float baseCooldownTime;
}