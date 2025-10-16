public class PlayerModel
{
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
}