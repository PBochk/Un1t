using UnityEngine;

public class GameInitiator : MonoBehaviour
{
    [SerializeField] private FloorManager floorManager;

    private void Awake()
    {
        floorManager.GenerateFloor();
        floorManager.GenerateRoomsContent();
    }
}