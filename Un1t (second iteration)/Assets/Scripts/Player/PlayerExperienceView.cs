using TMPro;
using UnityEngine;
using UnityEngine.UI;

//TODO: make UI class instead
public class PlayerExperienceView : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button health;
    [SerializeField] private Button attackSpeed;
    [SerializeField] private Button damage;
    [SerializeField] private PlayerMeleeWeaponModelMB meleeModelMB;
    private PlayerMeleeWeaponModel meleeModel;
    private PlayerModel model;
    private PlayerController controller;
    private void Awake()
    {
        health.onClick.AddListener(UpgradeHealth);
        attackSpeed.onClick.AddListener(UpgradeAttackSpeed);
        damage.onClick.AddListener(UpgradeDamage);
        controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        model = GetComponent<PlayerModelMB>().PlayerModel;
        model.NextLevel += OnLevelUp;
        meleeModel = (PlayerMeleeWeaponModel)meleeModelMB.MeleeWeaponModel;
    }

    // Experience model intialize later than OnEnable, so can't make it work rn
    // TODO: move subscription in OnEnable after model initialization rework
    //private void OnEnable()
    //{
    //    model.NextLevel += OnLevelUp;
    //}

    private void OnDisable()
    {
        model.NextLevel -= OnLevelUp;
    }

    private void OnLevelUp()
    {
        Debug.Log("OnLevelUp");
        canvas.gameObject.SetActive(true);
        controller.SetInputEnabled(false);
    }

    private void UpgradeHealth()
    {
        model.UpgradeHealth();
        DeactivateCanvas();
    }
    private void UpgradeAttackSpeed()
    { 
        meleeModel.UpgradeAttackSpeed(model.Level);
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
        controller.SetInputEnabled(true);
    }

}
