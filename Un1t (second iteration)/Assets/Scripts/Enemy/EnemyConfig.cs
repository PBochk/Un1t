using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Scriptable Objects/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [SerializeField]
    public int MaxHealth;
    [SerializeField]
    public float SpeedScale;
    [Tooltip("May be removed soon, to make config for each attack separately")]
    [SerializeField]
    public int Damage;
    [Tooltip("May be removed soon, in attacks per second")]
    [SerializeField]
    public float AttackSpeed;
    [SerializeField]
    public int XpGain;
}
