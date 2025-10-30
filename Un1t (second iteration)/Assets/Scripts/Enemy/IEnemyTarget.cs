using System;
using UnityEngine;

[Obsolete("Use EnemyTargetComponent instead")]
public interface IEnemyTarget
{
    public Vector2 Position { get; }
}