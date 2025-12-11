using UnityEngine;
using UnityEngine.Events;

public class Rock : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private int rocks;
    [SerializeField] private RocksSprites rocksSprites;

    public UnityEvent RockDestroyed;

    private void Awake()
    {
        GetComponent<Hitable>().HitTaken.AddListener(OnHitTaken);
        GetComponent<SpriteRenderer>().sprite = rocksSprites.GetRandomSprite();
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
