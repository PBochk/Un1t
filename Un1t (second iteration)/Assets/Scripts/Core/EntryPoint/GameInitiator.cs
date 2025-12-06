using UnityEngine;

public class GameInitiator : MonoBehaviour
{
    [SerializeField] private FloorManager floorManager;
    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
        floorManager.GenerateFloor();
        var target = playerController.GetComponent<EnemyTargetComponent>();
        floorManager.SetPlayer(playerController.gameObject);
        floorManager.GenerateRoomsContent();
    }
}