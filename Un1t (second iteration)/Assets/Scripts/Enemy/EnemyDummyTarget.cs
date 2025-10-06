using UnityEngine;

public class EnemyDummyTarget : MonoBehaviour, IEnemyTarget
{
    public Vector2 Position => transform.position;
}
