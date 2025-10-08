using UnityEngine;

/// <summary>
/// Creates and manages all rooms in the game level
/// </summary>
public class FloorManager : MonoBehaviour
{
    [SerializeField] private RoomInfo[] availableRooms;

    private const sbyte FLOOR_SIZE = 16;

    private readonly RoomInfo[,] rooms = new RoomInfo[FLOOR_SIZE, FLOOR_SIZE];


    void Awake()
    {
        GenerateFloor();
    }


    /// <summary>
    /// Creates all rooms for this level
    /// Each room gets it's own content
    /// </summary>
    private void GenerateFloor()
    {
        CreateRoom(ChooseRoom(default), new(FLOOR_SIZE/2, FLOOR_SIZE/2));
    }

    private void CreateRoom(RoomInfo room, Vector2Int floorGridPosition)
    {
        Instantiate(room.RoomPrefab, (Vector2)(floorGridPosition*RoomInfo.SIZE), Quaternion.identity, transform);
    }

    //TODO: optimize room choosing
    private RoomInfo ChooseRoom(RoomExits roomExits) 
    { 
        foreach (RoomInfo room in availableRooms)
        {
            if (room.Exits.Equals(roomExits))
                return room;
        }
        throw new System.Exception("Target room wasn't found");
    }

}