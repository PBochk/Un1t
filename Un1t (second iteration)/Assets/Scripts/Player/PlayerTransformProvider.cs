using UnityEngine;

public class PlayerTransformProvider : MonoBehaviour
{
    public static PlayerTransformProvider Instance { get; private set; }
    public Transform GetPlayerTransform() 
    {
        return transform;
    }
        
    private void Awake()
    {
        Instance = this;
    }
}