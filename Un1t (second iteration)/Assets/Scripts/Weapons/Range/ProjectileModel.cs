using UnityEngine;

public class ProjectileModel : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;
    private AttackData attackData;
    public float Lifetime => lifetime;
    public AttackData AttackData => attackData;

    private void Awake()
    {
        attackData = new AttackData(damage, DamageType.Physical);
    }
}
