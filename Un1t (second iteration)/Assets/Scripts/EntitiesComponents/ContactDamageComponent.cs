using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamageComponent : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float pushSpeed;
    [SerializeField] private int xpDamage;
    private bool isContactDamageReady = true;
    private const float contactDamageCooldown = 0.1f;
    private AttackData attackData;

    private void Awake()
    {
        attackData = new AttackData(damage, DamageType.Physical, xpDamage, transform, pushSpeed);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!isContactDamageReady) return;
        var otherGameObject = other.gameObject;
        if (otherGameObject.CompareTag("Player") && otherGameObject.TryGetComponent(out Hitable hitable))
        {
            hitable.TakeHit(attackData);
            StartCoroutine(WaitForCoooldown());
        }
    }

    private IEnumerator WaitForCoooldown()
    {
        isContactDamageReady = false;
        yield return new WaitForSeconds(contactDamageCooldown);
        isContactDamageReady = true;
    }
}