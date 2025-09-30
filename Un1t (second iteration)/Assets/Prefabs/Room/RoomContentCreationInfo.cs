using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class RoomContentCreationInfo
{
    public List<RoomEntity> Doors { get; } = new();
    //TODO: other RoomEntitie's Lists


    public RoomEntity[] GetContent()
    {
        List<RoomEntity> allGameEntities = new();

        PropertyInfo[] properties = GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            allGameEntities.AddRange(property.GetValue(this) as IEnumerable<RoomEntity>);
        }

        return allGameEntities.ToArray();
    }
}