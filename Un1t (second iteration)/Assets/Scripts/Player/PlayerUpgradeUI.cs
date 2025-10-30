using UnityEngine;
using UnityEngine.UI;

//TODO: make UI class instead
public class PlayerUpgradeUI : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button health;
    [SerializeField] private Button attackSpeed;
    [SerializeField] private Button damage;
    [SerializeField] private PlayerModelMB playerModelMB;
    [SerializeField] private PlayerMeleeWeaponModelMB meleeModelMB;
    private PlayerModel playerModel;
    private PlayerMeleeWeaponModel meleeModel;

    private void Awake()
    {
        health.onClick.AddListener(UpgradeHealth);
        attackSpeed.onClick.AddListener(UpgradeAttackSpeed);
        damage.onClick.AddListener(UpgradeDamage);
    }

    private void Start()
    {
        playerModel = playerModelMB.PlayerModel;
        playerModel.NextLevel += OnLevelUp;
        meleeModel = (PlayerMeleeWeaponModel)meleeModelMB.MeleeWeaponModel;
        canvas.gameObject.SetActive(false);

    }

    // Experience model intialize later than OnEnable, so can't make it work rn
    // TODO: move subscription in OnEnable after model initialization rework
    //private void OnEnable()
    //{
    //    playerModel.NextLevel += OnLevelUp;
    //}

    //private void OnDisable()
    //{
    //    playerModel.NextLevel -= OnLevelUp;
    //}

    private void OnLevelUp()
    {
        Debug.Log("OnLevelUp");
        canvas.gameObject.SetActive(true);
        playerModel.SetPlayerRestrained(true);
    }

    private void UpgradeHealth()
    {
        playerModel.UpgradeHealth();
        DeactivateCanvas();
    }
    private void UpgradeAttackSpeed()
    { 
        meleeModel.UpgradeAttackSpeed(playerModel.Level);
        DeactivateCanvas();
    }
    private void UpgradeDamage()
    {
        meleeModel.UpgradeDamage();
        DeactivateCanvas();
    }

    private void DeactivateCanvas()
    {
        canvas.gameObject.SetActive(false);
        playerModel.SetPlayerRestrained(false);
    }
}
