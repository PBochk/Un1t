using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUpgradeConfig", menuName = "Scriptable Objects/PlayerUpgrade")]
public class PlayerUpgradeConfig : ScriptableObject
{
    [SerializeField] private string description;
    [SerializeField] private float[] upgradeValues;
    public string Description => description;
    public float[] UpgradeValues => upgradeValues;
}
