using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Class made for containing state of player
/// </summary>
public class PlayerModelMB: MonoBehaviour, IActor
{
    [SerializeField] private PlayerConfig playerConfig;
    public PlayerModel PlayerModel { get; private set; }
    private const float REGEN_COOLDOWN = 5f;
    //private const float DODGE_COOLDOWN = 5f;
    private bool isRegenOnCooldown = false;
    //private bool isDodgeOnCooldown = false;
    private void Awake()
    {
        PlayerModel = new(playerConfig);
        isRegenOnCooldown = true;
    }

    private void Start()
    {
        PlayerModel.BindModels(GetComponentInChildren<PlayerMeleeWeaponModelMB>().MeleeWeaponModel as PlayerMeleeWeaponModel,
                               GetComponentInChildren<PlayerRangeWeaponModelMB>().RangeWeaponModel);
        Debug.Log(PlayerModel.MeleeModel is null);
        Debug.Log(PlayerModel.RangeModel is null);
    }

    public void Initialize(IInstanceModel model)
    {
        PlayerModel = model as PlayerModel;
    }

    private void Update()
    {
        if (!isRegenOnCooldown)
        {
            PlayerModel.Regenerate();
            StartCoroutine(WaitForRegenerationCooldown());
        }
        //if (!isDodgeOnCooldown)
        //{

        //}
    }

    private IEnumerator WaitForRegenerationCooldown()
    {
        isRegenOnCooldown = true;
        yield return new WaitForSeconds(REGEN_COOLDOWN);
        isRegenOnCooldown = false;
    }

    //private IEnumerator WaitForDodgeCooldown()
    //{
    //    isDodgeOnCooldown = true;
    //    yield return new WaitForSeconds(PlayerModel.DodgeCooldown);
    //    isDodgeOnCooldown = false;
    //}
}
