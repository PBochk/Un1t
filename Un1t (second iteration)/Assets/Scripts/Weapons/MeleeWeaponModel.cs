using UnityEngine;
/// <summary>
/// An abstract class made for containg state of weapon
/// </summary>
public abstract class MeleeWeaponModel : MonoBehaviour 
{
    [SerializeField] protected float damage;
    protected bool isAttackReady = true;
    protected bool isAttackActive = false;
    public float Damage => damage;
    public bool IsAttackReady => isAttackReady;
    public bool IsAttackActive => isAttackActive;
}
