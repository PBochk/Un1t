using System.Diagnostics;

public class PlayerModel
{
    private float health;
    public float Health => health;
    private float movingSpeed;
    private HealthComponent healthComponent;
    private PlayerExperienceModel experienceModel;
    public float MovingSpeed => movingSpeed;
    public HealthComponent HealthComponent => healthComponent;
    public PlayerExperienceModel ExperienceModel => experienceModel;

    public PlayerModel(float movingSpeed, HealthComponent healthComponent, PlayerExperienceModel experienceModel)
    {
        this.movingSpeed = movingSpeed;
        this.healthComponent = healthComponent;
        this.experienceModel = experienceModel;
    }

    public void TakeDamage(float decrement)
    {
        health -= decrement;
    }
}