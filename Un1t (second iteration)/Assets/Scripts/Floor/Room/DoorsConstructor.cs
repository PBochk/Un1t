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

    private readonly GameObject doorTile;

    private readonly bool topDoorExists;
    private readonly bool bottomDoorExists;
    private readonly bool leftDoorExists;
    private readonly bool rightDoorExists;

    private IEnumerable<GameObject> doors;

    private bool wereDoorsDestroyed = false;

    public DoorsConstructor(GameObject doorTile, RoomOuterWalls outerWalls)
    {
        this.doorTile = doorTile;

        topDoorExists = outerWalls.Top.Middle.IsEmpty;
        bottomDoorExists = outerWalls.Bottom.Middle.IsEmpty;
        leftDoorExists = outerWalls.Left.Middle.IsEmpty;
        rightDoorExists = outerWalls.Right.Middle.IsEmpty;
    }

    public void ConstructDoors(Transform room)
    {
        List<GameObject> doors = new();

        if (topDoorExists)
            doors.Add(CreateVerticalDoor(room, topDoorOffset));

        if (bottomDoorExists)
            doors.Add(CreateVerticalDoor(room, bottomDoorOffset));

        if (leftDoorExists)
            doors.Add(CreateHorizontalDoor(room, leftDoorOffset));

        if (rightDoorExists)
            doors.Add(CreateHorizontalDoor(room, rightDoorOffset));

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

    private GameObject CreateHorizontalDoor(Transform room, Vector3 offset)
    {
        GameObject horizontalDoor = GameObject.Instantiate(doorTile, offset + room.position, Quaternion.identity, room);
        horizontalDoor.GetComponent<SpriteRenderer>().size = horizontalDoorSize;
        horizontalDoor.GetComponent<BoxCollider2D>().size = horizontalDoorSize;
        return horizontalDoor;
    }

    private GameObject CreateVerticalDoor(Transform room, Vector3 offset)
    {
        GameObject verticalDoor = GameObject.Instantiate(doorTile, offset + room.position, Quaternion.identity, room);
        verticalDoor.GetComponent<SpriteRenderer>().size = verticalDoorSize;
        verticalDoor.GetComponent<BoxCollider2D>().size = verticalDoorSize;
        return verticalDoor;
    }
}
