using UnityEngine;

public abstract class MeleeWeaponModel : MonoBehaviour 
{
    public float Damage { get; set; }
    public bool IsAttackReady { get; set; } = true;
}
