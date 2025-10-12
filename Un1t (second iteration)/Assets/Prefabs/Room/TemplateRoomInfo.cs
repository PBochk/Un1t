using UnityEngine;

/// <summary>
///Room's template
/// </summary>
/// <see cref="RoomOuterWalls\">
[CreateAssetMenu(fileName = "RoomInfo", menuName = "Scriptable Objects/RoomInfo")]
public class TemplateRoomInfo : ScriptableObject
{
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

    private RoomInfo roomInfo;

    public RoomInfo Info => roomInfo ??= new RoomInfo(
        roomPrefab,
        leftTopIsEmpty, middleTopIsEmpty, rightTopIsEmpty,
        leftBottomIsEmpty, middleBottomIsEmpty, rightBottomIsEmpty,
        topLeftIsEmpty, middleLeftIsEmpty, bottomLeftIsEmpty,
        topRightIsEmpty, middleRightIsEmpty, bottomRightIsEmpty);
}