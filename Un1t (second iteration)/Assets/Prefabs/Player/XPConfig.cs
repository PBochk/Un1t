using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "XPConfig", menuName = "PlayerConfig/XP")]
public class XPConfig : ScriptableObject
{
    [SerializeField] private List<float> xpToNextLevel; // can't use IReadOnlyList because it doesn't support serialization
    public List<float> XPToNextLevel => xpToNextLevel;
}
