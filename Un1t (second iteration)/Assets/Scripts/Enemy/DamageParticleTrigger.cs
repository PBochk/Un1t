using UnityEngine;

[RequireComponent(typeof(EnemyModelMB))]
public class DamageParticleTrigger : MonoBehaviour
{
    [SerializeField] private ParticleSystem emitter;
    private EnemyModelMB model;

    public void Awake()
    {
        model = GetComponent<EnemyModelMB>();
        model.OnDamageTaken.AddListener(Trigger);
    }

    public void Trigger()
    {
        emitter.Play();
    }
}