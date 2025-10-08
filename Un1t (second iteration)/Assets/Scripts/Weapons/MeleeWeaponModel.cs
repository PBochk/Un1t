using UnityEngine;
/// <summary>
/// An abstract class made for containg state of weapon
/// </summary>
public abstract class MeleeWeaponModel : MonoBehaviour 
{
    [SerializeField] private float damage;
    public float Damage 
    { 
        get => damage; 
        set => damage = value; 
    }
    private bool isAttackReady = true;
    public bool IsAttackReady 
    { 
        get => isAttackReady; 
        protected set => isAttackReady = value;
    }
    public bool IsAttackActive { get; set; } = false;
}
