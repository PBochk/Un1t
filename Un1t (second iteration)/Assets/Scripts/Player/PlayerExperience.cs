using System;
using UnityEngine;

[RequireComponent(typeof(PlayerModel))]
[RequireComponent(typeof(PlayerExperienceModel))]
public class PlayerExperience : MonoBehaviour
{
    private PlayerModel playerModel;
    private PlayerExperienceModel model;
    public static PlayerExperience Instance;


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        model = GetComponent<PlayerExperienceModel>();
    }

    public void AddXP(float increment)
    {
        model.AddXP(increment);
    }
   

}
