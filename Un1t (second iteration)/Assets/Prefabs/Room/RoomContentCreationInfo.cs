using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Represents information about room content need to create
/// </summary>
public class RoomContentCreationInfo
{
    public List<RoomEntity> Doors { get; } = new();
    //TODO: other RoomEntitie's Lists


    /// <summary>Gets all room entities </summary>
    /// <remarks>This method finds all lists in this class and puts them in one array using reflection</remarks>
    /// <returns>Array with all room items</returns>
    public RoomEntity[] GetContent()
    {
        List<RoomEntity> allGameEntities = new();

        PropertyInfo[] properties = GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo property in properties)
            allGameEntities.AddRange((IEnumerable<RoomEntity>)property.GetValue(this));

        return allGameEntities.ToArray();
    }
}