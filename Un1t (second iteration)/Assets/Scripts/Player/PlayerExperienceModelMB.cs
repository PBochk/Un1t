using UnityEngine;

public class PlayerExperienceModelMB : MonoBehaviour
{
    public PlayerExperienceModel experienceModel;
    [SerializeField] private int level = 1;
    [SerializeField] private int xpCoefficient = 25;
    private void Awake()
    {
        // Temporary solution for serialization
        experienceModel = new PlayerExperienceModel(level, xpCoefficient);
    }
}
