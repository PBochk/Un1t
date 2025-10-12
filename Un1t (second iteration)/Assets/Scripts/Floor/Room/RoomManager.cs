using System.Collections.Generic;
using System.Collections.Immutable;
using UnityEngine;


/// <summary>
/// Manages everything inside one room
/// Keeps track of all RoomEntities
/// </summary>
public class RoomManager : MonoBehaviour
{
    private bool wasContentCreated = false;

    private ImmutableList<GameObject> entities;
    public IReadOnlyList<GameObject> Entities => entities;

    /// <summary>
    /// Creates all RoomEntities in this room 
    /// RoomEntities are placed relative to the room's position
    /// Shall only be called one time for each room
    /// </summary>
    public void CreateContent(RoomEntity[] roomEntities)
    {
        if (wasContentCreated)
            Debug.Log("Content of the room was created more than one time");

        ImmutableList<GameObject>.Builder entitiesBuilder = ImmutableList.CreateBuilder<GameObject>();

        foreach (RoomEntity entity in roomEntities)
        {
            Instantiate(entity.GameObject, entity.StartPosition + transform.position, Quaternion.identity, transform);
            entitiesBuilder.Add(entity.GameObject);
        }

        entities = entitiesBuilder.ToImmutable();

        wasContentCreated = true;
    }
}
