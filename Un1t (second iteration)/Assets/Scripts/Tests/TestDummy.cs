using UnityEngine;

public class TestDummy : MonoBehaviour
{
    [SerializeField] private float health = 10;

    private void Awake()
    {
        GetComponent<Hittable>().HitTaken.AddListener(TakeDamage);
    }

    private void TakeDamage(AttackData attackData)
    {
        health -= attackData.Damage;
        Debug.Log("Damage taken: " + attackData.Damage + " current hp: " + health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
