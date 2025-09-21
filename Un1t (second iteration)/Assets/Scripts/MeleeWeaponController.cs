//TODO: make melee weapon work
using UnityEngine;

public class MeleeWeaponController : MonoBehaviour
{
    private Animator anim;
    private MeleeWeaponModel model;
    [SerializeField] LayerMask enemy;

    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
        model = GetComponent<MeleeWeaponModel>();
    }
    private void FixedUpdate()
    {
        if (model.CurrentCooldown > 0)
        {
            model.CurrentCooldown -= Time.fixedDeltaTime;
        }
    }

    public void Attack()
    {
        if (model.CurrentCooldown <= 0)
        {
            Debug.Log("MeleeAttack");
            anim.SetTrigger("MeleeAttack");
            model.CurrentCooldown = model.AttackCooldown;
        }
    }

    public void OnAttack()
    {
        Debug.Log("OnMelee");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
