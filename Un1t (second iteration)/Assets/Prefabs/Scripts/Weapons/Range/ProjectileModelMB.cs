using UnityEngine;

public class ProjectileModelMB : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float lifetime;
    public ProjectileModel projectileModel;

    private void Awake()
    {
        projectileModel = new ProjectileModel(damage, lifetime);
    }
}
