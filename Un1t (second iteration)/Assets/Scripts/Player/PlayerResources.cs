using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources Instance;
    private PlayerRangeWeaponModel model;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        model = GetComponentInChildren<PlayerRangeWeaponModelMB>().RangeWeaponModel;
    }

    public void AddAmmo(int increment)
    {
        model.AddAmmo(increment);
    }
}
