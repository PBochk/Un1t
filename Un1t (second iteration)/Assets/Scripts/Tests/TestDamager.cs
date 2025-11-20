using UnityEngine;

public class TestDamager : MonoBehaviour
{
    [SerializeField] private float damage = 1.0f;
    [SerializeField] private int xpDamage = 1;

    private AttackData attackData;
    private void Awake()
    {
        attackData = new AttackData(damage, DamageType.Physical, xpDamage);
    }
    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        Debug.Log("OnTriggerEnter");
        if (collision.gameObject.TryGetComponent<Hitable>(out Hitable hittable))
        {
            hittable.TakeHit(attackData);
            Debug.Log(collision.gameObject.name + "took damage: " + damage);
        }
    }
}