using System;
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
    private ImmutableList<GameObject> outerWallFragments;

    private readonly static Range shurfesCountRange = new(2, 5);

    private int shurfesCount;

    public IReadOnlyList<GameObject> Entities => entities;
    public IReadOnlyList<GameObject> WallFragmentts => entities;


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
        ImmutableList<GameObject>.Builder wallFragmentsBuilder = ImmutableList.CreateBuilder<GameObject>();

        for (var i = 0; i < transform.childCount; i++)
        {
            GameObject wallFragment = transform.GetChild(i).gameObject;
            Instantiate(wallFragment, wallFragment.transform.position 
                + transform.position, Quaternion.identity, transform);
            wallFragment.GetComponent<OuterWallBuilderModel>().Create();
            wallFragmentsBuilder.Add(wallFragment);
        }

        outerWallFragments = wallFragmentsBuilder.ToImmutable();


        shurfesCount = UnityEngine.Random.Range(shurfesCountRange.Start.Value, shurfesCountRange.End.Value + 1);
      

        foreach (RoomEntity entity in roomEntities)
        {
            Instantiate(entity.GameObject, entity.StartPosition + transform.position, Quaternion.identity, transform);
            entitiesBuilder.Add(entity.GameObject);
        }

        entities = entitiesBuilder.ToImmutable();

        wasContentCreated = true;
    }
}
