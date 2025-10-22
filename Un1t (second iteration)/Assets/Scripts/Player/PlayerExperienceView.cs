using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperienceView : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button health;
    [SerializeField] private Button attackSpeed;
    [SerializeField] private Button damage;
    private PlayerModel model;
    private PlayerMeleeWeaponModel meleeModel;
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
        meleeModel = (PlayerMeleeWeaponModel)GetComponentInChildren<PlayerMeleeWeaponModelMB>().MeleeWeaponModel;
    }

    // Experience model intialize later than OnEnable, so can't make it work rn
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
        controller.DisableInput();
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
        controller.EnableInput();
    }

}
