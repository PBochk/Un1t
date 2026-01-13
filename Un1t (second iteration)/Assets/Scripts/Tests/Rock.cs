using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Hitable))]
[RequireComponent(typeof(SpriteRenderer))]
public class Rock : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private int rocks;

    [SerializeField] private RocksSprites floor1RocksSprites;
    [SerializeField] private RocksSprites floor2RocksSprites;
    [SerializeField] private RocksSprites floor3RocksSprites;

    public UnityEvent RockDestroyed;

    private void Awake()
    {
        GetComponent<Hitable>().HitTaken.AddListener(OnHitTaken);

        RocksSprites rocksSprites =
            FloorThemeManager.CurrentTheme == FloorTheme.Light
            ? floor1RocksSprites
            : FloorThemeManager.CurrentTheme == FloorTheme.Dark
            ? floor2RocksSprites
            : floor3RocksSprites;
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
