using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "PlayerConfig/Player")]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] private float baseMaxHealth;
    [SerializeField] private float baseMovingSpeed;
    
    public float BaseMaxHealth => baseMaxHealth;
    public float BaseMovingSpeed => baseMovingSpeed;
}
