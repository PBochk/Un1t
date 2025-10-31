using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PlayerModelMB playerModelMB;
    [SerializeField] private PlayerMeleeWeaponModelMB playerMeleeModelMB;
    [SerializeField] private PlayerRangeWeaponModelMB playerRangeModelMB;
    public PlayerModelMB PlayerModelMB => playerModelMB;
    public PlayerMeleeWeaponModelMB PlayerMeleeWeaponModelMB => playerMeleeModelMB;
    public PlayerRangeWeaponModelMB PlayerRangeWeaponModelMB => playerRangeModelMB;

}
