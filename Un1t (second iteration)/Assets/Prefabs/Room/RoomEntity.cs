using UnityEngine;

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
