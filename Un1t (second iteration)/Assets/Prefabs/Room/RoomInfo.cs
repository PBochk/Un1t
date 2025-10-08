using System;
using UnityEngine;

/// <summary>
///Room's outer walls description
/// </summary>
/// <see cref="RoomOuterWalls\">
[CreateAssetMenu(fileName = "RoomInfo", menuName = "Scriptable Objects/RoomInfo")]
public class RoomInfo : ScriptableObject
{
    public static Vector2Int SIZE = new(16, 9);

    [Header("Room's outer walls description. Mark if it's part is empty")]
    [SerializeField] private GameObject roomPrefab;

    [Space]
    [Header("Top Wall")]
    [SerializeField] private bool leftTopIsEmpty;
    [SerializeField] private bool middleTopIsEmpty;
    [SerializeField] private bool rightTopIsEmpty;

    [Space]
    [Header("Bottom Wall")]
    [SerializeField] private bool leftBottomIsEmpty;
    [SerializeField] private bool middleBottomIsEmpty;
    [SerializeField] private bool rightBottomIsEmpty;

    [Space]
    [Header("Left Wall")]
    [SerializeField] private bool topLeftIsEmpty;
    [SerializeField] private bool middleLeftIsEmpty;
    [SerializeField] private bool bottomLeftIsEmpty;

    [Space]
    [Header("Right Wall")]
    [SerializeField] private bool topRightIsEmpty;
    [SerializeField] private bool middleRightIsEmpty;
    [SerializeField] private bool bottomRightIsEmpty;

    public GameObject RoomPrefab => roomPrefab;

    private RoomOuterWalls? outerWalls;
    private RoomExits? roomExits;

    /// <summary>
    /// Gets the complete description of all outer walls of the room
    /// Creates a RoomOuterWalls object with information about each wall part
    /// </summary>
    public RoomOuterWalls OuterWalls => outerWalls
        ?? (outerWalls = new(
        new RoomOuterWalls.Wall(
            new RoomOuterWalls.Wall.WallPart(leftTopIsEmpty),
            new RoomOuterWalls.Wall.WallPart(middleTopIsEmpty),
            new RoomOuterWalls.Wall.WallPart(rightTopIsEmpty)
        ),
        new RoomOuterWalls.Wall(
            new RoomOuterWalls.Wall.WallPart(leftBottomIsEmpty),
            new RoomOuterWalls.Wall.WallPart(middleBottomIsEmpty),
            new RoomOuterWalls.Wall.WallPart(rightBottomIsEmpty)
        ),
        new RoomOuterWalls.Wall(
            new RoomOuterWalls.Wall.WallPart(topLeftIsEmpty),
            new RoomOuterWalls.Wall.WallPart(middleLeftIsEmpty),
            new RoomOuterWalls.Wall.WallPart(bottomLeftIsEmpty)
        ),
        new RoomOuterWalls.Wall(
            new RoomOuterWalls.Wall.WallPart(topRightIsEmpty),
            new RoomOuterWalls.Wall.WallPart(middleRightIsEmpty),
            new RoomOuterWalls.Wall.WallPart(bottomRightIsEmpty)
        )
    )).Value;

    public RoomExits Exits => roomExits
        ?? (roomExits = CalculateRoomExits(OuterWalls)).Value;

    private static RoomExits CalculateRoomExits(RoomOuterWalls roomOuterWalls)
    {
        static bool checkWallExit(RoomOuterWalls.Wall wall) =>
            wall.First.IsEmpty || wall.Middle.IsEmpty || wall.Last.IsEmpty;

        return new(checkWallExit(roomOuterWalls.Top), checkWallExit(roomOuterWalls.Bottom), 
            checkWallExit(roomOuterWalls.Left), checkWallExit(roomOuterWalls.Right));
    }


}