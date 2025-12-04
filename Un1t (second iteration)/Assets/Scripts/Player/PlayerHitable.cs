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
        // TODO: move lines below to OnEnable after initialization rework
        playerModel.ShieldUnlocked += () => hasShield = true; // activate shield after unlock
        hasShield = playerModel.ShieldCooldown != 0; // activate shield after loading if it was unlocked before 
    }

    private void OnDisable()
    {
        playerModel.ShieldUnlocked -= () => hasShield = true;
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
    /// Set shield on cooldown
    /// </summary>
    private IEnumerator WaitForShieldCooldown()
    {
        hasShield = false;
        yield return new WaitForSeconds(playerModel.ShieldCooldown);
        hasShield = true;
    }
}