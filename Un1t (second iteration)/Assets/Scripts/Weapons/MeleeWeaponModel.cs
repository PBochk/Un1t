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
    public bool IsAttackReady { get; protected set; } = true;
    public bool IsAttackActive { get; set; } = false;
}
