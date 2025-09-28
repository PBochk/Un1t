using UnityEngine;
using System.Collections.Generic;

public class PlayerMeleeWeaponController : MeleeWeaponController
{
    private PlayerController playerController;

    protected override void Awake()
    {
        model = GetComponent<PlayerMeleeWeaponModel>();
        playerController = GetComponentInParent<PlayerController>();
        playerController.onMeleeAttack.AddListener(OnMeleeAttack);
        playerController.onMeleeAttackStart.AddListener(StartMeleeAttack);
        base.Awake();
    }
}
