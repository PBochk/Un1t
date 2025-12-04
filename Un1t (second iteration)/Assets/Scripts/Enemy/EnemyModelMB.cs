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

    //TODO: Make an event for values changing for view/animator to update animation speed
    //TODO: Think if we should pass any parameters here
    //or to remove this event even
    //TODO: Event naming
    public UnityEvent OnDamageTaken;
    public UnityEvent OnDeath;

    private Hitable hitable;
    private ExperienceComponent experienceComponent;

    private void Awake()
    {
        NativeModel = new(config.MaxHealth, config.InitialSpeedCoefficient);
        hitable = GetComponent<Hitable>();
        hitable.HitTaken.AddListener(Damage);
        experienceComponent = GetComponent<ExperienceComponent>();
        experienceComponent.SetXP(config.XpGain);
        OnDeath.AddListener(experienceComponent.OnDropXP);
    }

    //This stays public for now but may become private
    public void Damage(AttackData data)
    {
        //TODO: There could be damage calculation logic
        NativeModel.Damage(data);
        OnDamageTaken.Invoke();
        //Debug.Log($"Current HP: {NativeModel.Hp}");
        CheckHp();
    }

    private void CheckHp()
    {
        if (NativeModel.Hp <= 0)
        {
            OnDeath.Invoke();
        }
    }
    
    //To be implemented 
    //public void Heal()
}