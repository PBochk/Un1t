using UnityEngine;

/// <summary>
/// A class for testing enemy capability to follow its target
/// </summary>
public class EnemyDummyTarget : MonoBehaviour, IEnemyTarget
{
    public Vector2 Position => transform.position;
}
