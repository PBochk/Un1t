using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public static PlayerResources Instance;
    private PlayerRangeWeaponModel model;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        model = GetComponentInChildren<PlayerRangeWeaponModelMB>().playerRangeWeaponModel;
    }

    public void AddAmmo(int increment)
    {
        model.AddAmmo(increment);
        Debug.Log(model.Ammo);
    }


}
