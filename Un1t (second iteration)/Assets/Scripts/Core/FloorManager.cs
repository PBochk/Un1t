using Unity.Cinemachine;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public GameObject room;
    public int roomCount = 5;
    public float roomSpacing = 16f;

    public CinemachineConfiner2D confiner2D;

    private GameObject[] rooms;
    private int currentRoomIndex = 0;

    void Awake()
    {
        GenerateFloor();
        SetCurrentRoom(0);
    }

    void GenerateFloor()
    {
        rooms = new GameObject[roomCount];

        for (int i = 0; i < roomCount; i++)
        {
            Vector3 roomPosition = new(i * roomSpacing, 0, 0);
            GameObject roomObj = Instantiate(room, roomPosition, Quaternion.identity, transform);

            rooms[i] = roomObj;
        }

    }

    public void SetCurrentRoom(int roomIndex)
    {
        if (roomIndex < 0 || roomIndex >= rooms.Length) return;

        currentRoomIndex = roomIndex;
        GameObject currentRoom = rooms[roomIndex];

        // Обновляем Cinemachine Confiner
        if (confiner2D != null)
        {
            Collider2D roomCollider = currentRoom.GetComponent<Collider2D>();
            if (roomCollider != null)
            {
                confiner2D.BoundingShape2D = roomCollider;
            }
        }
    }

    public void MoveToNextRoom()
    {
        if (currentRoomIndex < roomCount - 1)
        {
            SetCurrentRoom(currentRoomIndex + 1);
        }
    }

    public void MoveToPreviousRoom()
    {
        if (currentRoomIndex > 0)
        {
            SetCurrentRoom(currentRoomIndex - 1);
        }
    }

    public GameObject GetCurrentRoom()
    {
        return rooms[currentRoomIndex];
    }

    public int GetCurrentRoomIndex()
    {
        return currentRoomIndex;
    }

}