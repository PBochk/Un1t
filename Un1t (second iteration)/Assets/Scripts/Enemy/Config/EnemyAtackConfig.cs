using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "EnemyConfig/EnemyAttack")]
public class EnemyAtackConfig : ScriptableObject
{
    [SerializeField] private float damage;
    [SerializeField] private DamageType damageType;
    //Unused rn
    [SerializeField] private float baseCooldownTime;

    private AttackData attackData;

    public AttackData AttackData => attackData;
    public float BaseCooldownTime => baseCooldownTime;

    private void Awake()
    {
        attackData = new AttackData(damage, damageType);
    }
}