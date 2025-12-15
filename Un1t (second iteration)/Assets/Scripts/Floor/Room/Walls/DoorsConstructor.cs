using System.Collections.Generic;
using UnityEngine;

public class DoorsConstructor
{
    private static readonly Vector3 topDoorOffset = new(0, 8);
    private static readonly Vector3 bottomDoorOffset = new(0, -8);
    private static readonly Vector3 leftDoorOffset = new(-10, 0);
    private static readonly Vector3 rightDoorOffset = new(10, 0);

    private static readonly Vector2 horizontalDoorSize = new(2, 4);
    private static readonly Vector2 verticalDoorSize = new(6, 2);

    private static readonly Vector2 minimumColliderSize = new(4, 4);

    private readonly GameObject horizontalDoorPrefab;
    private readonly GameObject verticalDoorPrefab;

    private readonly bool topDoorExists;
    private readonly bool bottomDoorExists;
    private readonly bool leftDoorExists;
    private readonly bool rightDoorExists;

    private IEnumerable<GameObject> doors;

    private bool wereDoorsDestroyed = false;

    public DoorsConstructor(FloorObjectsList floorObjectsList, RoomOuterWalls outerWalls)
    {
        horizontalDoorPrefab = floorObjectsList.HorizontalDoor;
        verticalDoorPrefab = floorObjectsList.VerticalDoor;

        topDoorExists = outerWalls.Top.Middle.IsEmpty;
        bottomDoorExists = outerWalls.Bottom.Middle.IsEmpty;
        leftDoorExists = outerWalls.Left.Middle.IsEmpty;
        rightDoorExists = outerWalls.Right.Middle.IsEmpty;
    }

    public void ConstructDoors(Transform room)
    {
        List<GameObject> doors = new();

        if (topDoorExists)
            doors.Add(CreateVerticalDoor(room, topDoorOffset, Direction.Top));

        if (bottomDoorExists)
            doors.Add(CreateVerticalDoor(room, bottomDoorOffset, Direction.Bottom));

        if (leftDoorExists)
            doors.Add(CreateHorizontalDoor(room, leftDoorOffset, Direction.Left));

        if (rightDoorExists)
            doors.Add(CreateHorizontalDoor(room, rightDoorOffset, Direction.Right));

        this.doors = doors;
    }

    public void DestroyDoors()
    {
        if (wereDoorsDestroyed)
        {
            Debug.LogWarning("Doors were already destroyed");
            return;
        }
        foreach (GameObject door in doors)
            GameObject.Destroy(door);
        wereDoorsDestroyed = true;
    }

    private GameObject CreateHorizontalDoor(Transform room, Vector3 offset, Direction direction)
    {
        GameObject horizontalDoor = GameObject.Instantiate(verticalDoorPrefab, offset + room.position, Quaternion.identity, room);
        horizontalDoor.GetComponent<SpriteRenderer>().size = horizontalDoorSize;

        BoxCollider2D collider = horizontalDoor.GetComponent<BoxCollider2D>();

        collider.size = new Vector2(Mathf.Max(horizontalDoorSize.x, minimumColliderSize.x),
                                   Mathf.Max(horizontalDoorSize.y, minimumColliderSize.y));

        collider.offset = new Vector2(direction == Direction.Left ? -1 : 1, 0);

        return horizontalDoor;
    }

    private GameObject CreateVerticalDoor(Transform room, Vector3 offset, Direction direction)
    {
        GameObject verticalDoor = GameObject.Instantiate(horizontalDoorPrefab, offset + room.position, Quaternion.identity, room);
        verticalDoor.GetComponent<SpriteRenderer>().size = verticalDoorSize;

        BoxCollider2D collider = verticalDoor.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(Mathf.Max(verticalDoorSize.x, minimumColliderSize.x),
                                   Mathf.Max(verticalDoorSize.y, minimumColliderSize.y));
        collider.offset = new Vector2(0, direction == Direction.Bottom ? -1 : 1);
        return verticalDoor;
    }

    private enum Direction { Top, Bottom, Left, Right }
}