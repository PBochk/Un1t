using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "EnemyConfig/Enemy")]
public class EnemyConfig : ScriptableObject
{
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float initialSpeedCoefficient;
    [SerializeField]
    private int xpGain;
    [Tooltip("In units per second, doesn't impact on cooldown")]
    [SerializeField] private float baseMoveSpeed;
    [Tooltip("In units")] [SerializeField] private float aggroRange;
    
    public float BaseMoveSpeed => baseMoveSpeed;
    public float MaxHealth => maxHealth;
    public float InitialSpeedCoefficient => initialSpeedCoefficient;
    public int XpGain => xpGain;
    public float AggroRange => aggroRange;
}
