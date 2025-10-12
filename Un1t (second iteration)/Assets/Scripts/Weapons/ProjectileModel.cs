using UnityEngine;

public class ProjectileModel : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;

    public float Damage => damage;
    public float Lifetime => lifetime;
}
