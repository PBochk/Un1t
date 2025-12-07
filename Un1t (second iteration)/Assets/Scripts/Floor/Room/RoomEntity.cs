using UnityEngine;
public class RoomEntity
{
    public GameObject GameObject { get; }

    public Vector2 StartPosition { get; }

    public RoomEntity(GameObject gameObject, Vector2 startPosition)
    {
        GameObject = gameObject;
        StartPosition = startPosition;

    }
}
