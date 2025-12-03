using UnityEngine;
public class RoomEntity
{
    public GameObject GameObject { get; }

    public Vector2Int StartPosition { get; }


    public RoomEntity(GameObject gameObject, Vector2Int startPosition)
    {
        GameObject = gameObject;
        StartPosition = startPosition;

    }
}
