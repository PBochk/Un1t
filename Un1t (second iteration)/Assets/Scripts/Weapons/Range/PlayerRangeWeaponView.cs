using UnityEngine;

public class PlayerRangeWeaponView : MonoBehaviour
{
    [SerializeField] private ToolsUI toolsUI;
    private PlayerRangeWeaponModelMB modelMB;
    private PlayerRangeWeaponModel model;

    private void Awake()
    {
        modelMB = GetComponent<PlayerRangeWeaponModelMB>();
    }

    private void Start()
    {
        // TODO: move subscription to OnEnable after model initialization rework 
        model = modelMB.playerRangeWeaponModel;
        model.AmmoChanged += OnAmmoChanged;
        Initialize();
    }

    private void OnDisable()
    {
        model.AmmoChanged -= OnAmmoChanged;
    }

    private void Initialize()
    {
        OnAmmoChanged();
    }

    private void OnAmmoChanged()
    {
        toolsUI.AmmoText.text = model.Ammo.ToString();
    }
}
