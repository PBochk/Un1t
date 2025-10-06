using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    public UnityEvent Death;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public float Health => currentHealth;

    public void TakeHeal(float heal)
    {
        currentHealth = (currentHealth + heal <= maxHealth) ? currentHealth + heal : maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        CheckHealth();
        Debug.Log("Damage taken: " + damage + " by entity: " + name + " current hp: " + currentHealth);
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Death?.Invoke();
            Destroy(gameObject);
        }
    }
}
