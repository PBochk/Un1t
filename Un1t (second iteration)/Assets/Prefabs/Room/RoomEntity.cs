using UnityEngine;

/// <summary>
/// Represents one object in a room
/// Contains the object and its position in the room
/// </summary>
public class RoomEntity
{
    public GameObject GameObject { get; }

    public Vector3 StartPosition { get; }


    public RoomEntity(GameObject gameObject, Vector3 startPosition)
    {
        GameObject = gameObject;
        StartPosition = startPosition;

    }
}
