using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerModelMB))]
public class PlayerHitable : Hitable
{
    [SerializeField] private float invulTime;
    private PlayerModel playerModel;
    private bool isVulnerable = true;
    /// <summary>
    /// Player ingores next hit if has shield
    /// </summary>
    private bool hasShield = false;

    private void Start()
    {
        playerModel = GetComponent<PlayerModelMB>().PlayerModel;
        hasShield = playerModel.ShieldCooldown != 0; // TODO: move to OnEnable after initialization rework
    }


    public override void TakeHit(AttackData attackData)
    {
        if (!isVulnerable) return;
        if (playerModel.DodgeChance >= Random.Range(0, 1f)) return;
        StartCoroutine(WaitForInvulnerability(invulTime));
        if (hasShield)
        {
            Debug.Log("Shield");
            StartCoroutine(WaitForShieldCooldown());
            return;
        }
        base.TakeHit(attackData);
    }

    /// <summary>
    /// Method for starting player's invulnerability from external classes
    /// </summary>
    /// <param name="invulTime"></param>
    public void SetInvulForSeconds(float invulTime) => StartCoroutine(WaitForInvulnerability(invulTime));

    /// <summary>
    /// Makes player temporarily invulnerable
    /// </summary>
    private IEnumerator WaitForInvulnerability(float invulTime)
    {
        isVulnerable = false;
        yield return new WaitForSeconds(invulTime);
        isVulnerable = true;
    }

    /// <summary>
    /// Activate shield after unlock
    /// </summary>
    public void SetShieldActive()
    {
        hasShield = true;
    }

    /// <summary>
    /// Set shield on cooldown
    /// </summary>
    private IEnumerator WaitForShieldCooldown()
    {
        hasShield = false;
        yield return new WaitForSeconds(playerModel.ShieldCooldown);
        hasShield = true;
    }
}