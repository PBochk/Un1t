using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PlayerUpgradeConfig", menuName = "Scriptable Objects/PlayerUpgrade")]
public class PlayerUpgradeConfig : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private string upgradeName;
    [SerializeField] private string description;
    [SerializeField] private float[] upgradeValues;
    public Sprite Icon => icon;
    public string Description => description;
    public string Name => upgradeName;
    public float[] UpgradeValues => upgradeValues;
}
