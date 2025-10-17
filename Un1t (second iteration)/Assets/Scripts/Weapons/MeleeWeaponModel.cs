using UnityEngine;
/// <summary>
/// An abstract class made for containg state of weapon
/// </summary>
public abstract class MeleeWeaponModel : MonoBehaviour 
{
    [SerializeField] protected float damage;
    [SerializeField] protected DamageType damageType = DamageType.Physical;
    private AttackData attackData;
    public AttackData AttackData => attackData;
    protected bool isAttackReady = true;
    protected bool isAttackActive = false;
    public bool IsAttackReady => isAttackReady;
    public bool IsAttackActive => isAttackActive;

    public void StartActive()
    {
        isAttackActive = true;
    }
    public void EndActive()
    {
        isAttackActive = false;
    }

    private void Awake()
    {
        attackData = new AttackData(damage, damageType, gameObject);
    }

}
