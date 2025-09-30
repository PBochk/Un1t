using System.Collections.Generic;
//using System.Collections.Immutable;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private bool wasContentCreated = false;

    private List<GameObject> entities = new();
    public IReadOnlyList<GameObject> Entities => entities;


    public void CreateContent(RoomEntity[] roomEntities)
    {
        if (wasContentCreated)
            Debug.Log("Content of the room was created more than one time");

        foreach (var entity in roomEntities)
        {
            Instantiate(entity.GameObject, entity.StartPosition + transform.position, Quaternion.identity, transform);
            entities.Add(entity.GameObject);
        }


        wasContentCreated = true;
    }
}
