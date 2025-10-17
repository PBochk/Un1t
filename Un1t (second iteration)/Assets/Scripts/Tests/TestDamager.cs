using UnityEngine;

public class TestDamager : MonoBehaviour
{
    private float damage = 1.0f;
    private AttackData attackData;
    private void Awake()
    {
        attackData = new AttackData(damage, DamageType.Physical);
    }
    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        Debug.Log("OnTriggerEnter");
        if (collision.gameObject.TryGetComponent<Hitable>(out Hitable hittable))
        {
            hittable.HitTaken.Invoke(attackData);
            Debug.Log(collision.gameObject.name + "took damage: " + damage);
        }
    }
}