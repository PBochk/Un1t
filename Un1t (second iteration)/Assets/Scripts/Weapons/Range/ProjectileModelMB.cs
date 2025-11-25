using UnityEngine;

public class ProjectileModelMB : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;
    
    private ProjectileModel projectileModel;
    public ProjectileModel ProjectileModel => projectileModel;

    private void Awake()
    {
        projectileModel = new ProjectileModel(damage, lifetime);
    }
}
