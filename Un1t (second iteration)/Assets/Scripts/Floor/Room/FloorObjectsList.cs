using UnityEngine;

[CreateAssetMenu(fileName = "FloorObjectsList", menuName = "Scriptable Objects/FloorObjectsList", order = 1)]
public class FloorObjectsList : ScriptableObject
{
    public GameObject RoomTemplate => roomTemplate;
    public GameObject HorizontalHallway => horizontalHallway;
    public GameObject VerticalHallway => verticalHallway;
    public GameObject TopOuterWall => topOuterWall;
    public GameObject BottomOuterWall => bottomOuterWall;
    public GameObject LeftOuterWall => leftOuterWall;
    public GameObject RightOuterWall => rightOuterWall;
    public GameObject Rock => rock;
    public GameObject Descent => descent;
    public GameObject HorizontalDoor => horizontalDoor;
    public GameObject VerticalDoor => verticalDoor;

    [SerializeField] private GameObject roomTemplate;

    [SerializeField] private GameObject horizontalHallway;
    [SerializeField] private GameObject verticalHallway;

    [SerializeField] private GameObject topOuterWall;
    [SerializeField] private GameObject bottomOuterWall;
    [SerializeField] private GameObject leftOuterWall;
    [SerializeField] private GameObject rightOuterWall;

    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject descent;

    [SerializeField] private GameObject horizontalDoor;
    [SerializeField] private GameObject verticalDoor;
}