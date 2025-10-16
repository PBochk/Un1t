using UnityEngine;

[RequireComponent(typeof(PlayerModelMB))]
[RequireComponent(typeof(PlayerExperienceModelMB))]
public class PlayerExperience : MonoBehaviour
{
    private PlayerExperienceModel model;
    public static PlayerExperience Instance;


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        model = GetComponent<PlayerExperienceModelMB>().experienceModel;
    }

    public void AddXP(int increment)
    {
        model.AddXP(increment);
    }
   

}
