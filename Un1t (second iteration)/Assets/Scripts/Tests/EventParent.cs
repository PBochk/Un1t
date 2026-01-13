using System;
using UnityEngine;
using UnityEngine.Events;
//This class is for demo only.
[Obsolete]
public class EventParent : MonoBehaviour
{
    [Header("This script is for demo only")]
    public UnityEvent LevelEnded;
    public void NotifyLevelEnded()
    {
        LevelEnded?.Invoke();
    }
}
