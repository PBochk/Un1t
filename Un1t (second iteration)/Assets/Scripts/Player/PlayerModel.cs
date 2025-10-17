public class PlayerModel
{
    private float health;
    public float Health => health;
    private float movingSpeed;
    private PlayerExperienceModel experienceModel;
    public float MovingSpeed => movingSpeed;
    public PlayerExperienceModel ExperienceModel => experienceModel;

    public PlayerModel(float movingSpeed, PlayerExperienceModel experienceModel)
    {
        this.movingSpeed = movingSpeed;
        this.experienceModel = experienceModel;
    }

    public void TakeDamage(float decrement)
    {
        health -= decrement;
    }
}