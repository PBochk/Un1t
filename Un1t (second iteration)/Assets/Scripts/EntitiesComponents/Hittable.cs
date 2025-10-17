using UnityEngine;
using UnityEngine.Events;

public class Hittable : MonoBehaviour
{
    public UnityEvent<AttackData> HitTaken;
}
