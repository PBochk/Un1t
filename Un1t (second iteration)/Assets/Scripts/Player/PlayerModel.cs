using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class made for containg state of player
/// </summary>
public class PlayerModel: MonoBehaviour
{
    [SerializeField] private float movingSpeed;
    //[SerializeField] private float maxHealthIncrease;
    //[SerializeField] private float damageIncrease;
    //[SerializeField] private float attackSpeedIncrease;
    [SerializeField] private HealthComponent healthComponent;
    [SerializeField] private PlayerExperienceModel experienceModel;
    public float MovingSpeed => movingSpeed;
    public HealthComponent HealthComponent => healthComponent;
    public PlayerExperienceModel ExperienceModel => experienceModel;

}
