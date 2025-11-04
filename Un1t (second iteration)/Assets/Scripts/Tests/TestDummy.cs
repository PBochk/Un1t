using UnityEngine;
using UnityEngine.Events;

public class TestDummy : MonoBehaviour
{
    [SerializeField] private float health = 10;
    public UnityEvent Death;
    private void Awake()
    {
        GetComponent<Hitable>().HitTaken.AddListener(TakeDamage);
        Death.AddListener(GetComponent<ExperienceComponent>().OnDropXP);
    }

    private void TakeDamage(AttackData attackData)
    {
        health -= attackData.Damage;
        Debug.Log("Damage taken: " + attackData.Damage + " current hp: " + health);
        if (health <= 0)
        {
            Death?.Invoke();
            Destroy(gameObject);
        }
    }
}
