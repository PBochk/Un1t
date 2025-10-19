using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Hitable))]
[RequireComponent(typeof(ExperienceComponent))]
public class EnemyModelMB : MonoBehaviour
{
    [SerializeField] private EnemyConfig config;
    
    //TODO: fix public field use
    public EnemyModel NativeModel;
    public EnemyConfig Config => config;

    //TODO: Think if we should pass any parameters here
    //or to remove this event even
    public UnityEvent OnDamageTaken;
    public UnityEvent OnDeath;

    private void Awake()
    {
        NativeModel = new(config.MaxHealth, config.InitialSpeedCoefficient);
    }

    public void Damage(AttackData data)
    {
        //TODO: There could be damage calculation logic
        NativeModel.Damage(data);
        OnDamageTaken.Invoke();
    }
    
    //To be implemented 
    //public void Heal()
}