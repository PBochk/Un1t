using UnityEngine;

/// <summary>
/// Creates and manages all rooms in the game level
/// </summary>
public class FloorManager : MonoBehaviour
{
    [SerializeField] private GameObject room;
    [SerializeField] private GameObject door;

    private int roomCount;


    void Awake()
    {
        roomCount = 5;

        GenerateFloor();
    }


    /// <summary>
    /// Creates all rooms for this level
    /// Each room gets it's own content
    /// </summary>
    private void GenerateFloor()
    {
        for (var i = 0; i < roomCount; i++)
        {
            Vector3 roomPosition = new(i * 16f, 0, 0);
            GameObject roomObject = Instantiate(room, roomPosition, Quaternion.identity, transform);
            RoomManager roomManager = roomObject.GetComponent<RoomManager>();
            RoomContentCreationInfo roomContent = new();

            roomManager.CreateContent(roomContent.GetContent());
        }
    }

}