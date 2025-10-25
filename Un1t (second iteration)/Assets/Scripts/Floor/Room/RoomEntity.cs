using UnityEngine;

/// <summary>
/// Represents one object in a room
/// Contains the object and its position in the room
/// </summary>
public abstract class RoomEntity : MonoBehaviour  
{
    public GameObject GameObject { get; }

    public Vector3 StartPosition { get; }

    public abstract void Create();

    public RoomEntity(GameObject gameObject, Vector3 startPosition)
    {
        GameObject = gameObject;
        StartPosition = startPosition;

    }
}

public class EnemyEntity : RoomEntity
{
    protected override void Create()
    {

    }
}