using UnityEngine;

/// <summary>
/// Class made for containg state of player
/// </summary>
public class PlayerModel: MonoBehaviour
{
    [SerializeField] private float movingSpeed;
    [SerializeField] private HealthComponent healthComponent;
    
    public float MovingSpeed => movingSpeed;
    public HealthComponent HealthComponent => healthComponent;
}
