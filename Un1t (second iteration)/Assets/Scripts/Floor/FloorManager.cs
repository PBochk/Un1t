using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] private GameObject room;
    [SerializeField] private GameObject door;


    private int roomCount = 5;
    private float roomSpacing = 16f;

    private GameObject[] rooms;

    void Awake()
    {
        GenerateFloor();
    }

    void GenerateFloor()
    {
        rooms = new GameObject[roomCount];

        for (var i = 0; i < roomCount; i++)
        {
            Vector3 roomPosition = new(i * roomSpacing, 0, 0);
            GameObject roomObject = Instantiate(room, roomPosition, Quaternion.identity, transform);

            RoomManager roomManager = roomObject.GetComponent<RoomManager>();

            RoomContentCreationInfo roomContent = new();
            roomContent.Doors.Add(new (door, new (8, 0, 0)));


            roomManager.CreateContent(roomContent.GetContent());
        }

    }

}