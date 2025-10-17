using UnityEngine;
/// <summary>
/// An abstract class made for containg state of weapon
/// </summary>
public abstract class MeleeWeaponModelMB : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected DamageType damageType = DamageType.Physical;
    private MeleeWeaponModel meleeWeaponModel;
    public MeleeWeaponModel MeleeWeaponModel => meleeWeaponModel;

    protected bool isAttackReady = true;
    protected bool isAttackActive = false;
    public bool IsAttackReady => isAttackReady;
    public bool IsAttackActive => isAttackActive;

    protected virtual void Awake() 
    {
        meleeWeaponModel = new MeleeWeaponModel(damage, damageType);
    }

    public void StartActive()
    {
        isAttackActive = true;
    }
    public void EndActive()
    {
        isAttackActive = false;
    }

}
