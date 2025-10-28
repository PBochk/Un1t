using UnityEngine;
using UnityEngine.Events;

public class Rock : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private int rocks;
    public UnityEvent RockDestroyed;
    private void Awake()
    {
        GetComponent<Hitable>().HitTaken.AddListener(OnHitTaken);
    }

    private void OnHitTaken(AttackData attackData)
    {
        health -= attackData.Damage;
        CheckHealth();
    }

    private void CheckHealth()
    {
        if(health <= 0)
        {
            DestroyRock();
        }
    }

    private void DestroyRock()
    {
        RockDestroyed?.Invoke();
        PlayerResources.Instance.AddAmmo(rocks);
        Destroy(gameObject);
    }
}
