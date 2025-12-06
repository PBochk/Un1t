using System;

public class PlayerRangeWeaponModel
{
    private float damage;
    private float lifetime;
    public ProjectileModel ProjectileModel;

    private float attackCooldown;
    private int ammo;

    public float AttackCooldown => attackCooldown;
    public event Action AmmoChanged;
    public int Ammo
    {
        get => ammo;
        private set
        {
            ammo = value;
            AmmoChanged?.Invoke();
        }
    }

    // TODO: remade with scriptable object
    public PlayerRangeWeaponModel(float damage, float lifetime, float attackCooldown, int ammo)
    {
        this.damage = damage;
        this.lifetime = lifetime;
        ProjectileModel = new(damage, lifetime);
        this.attackCooldown = attackCooldown;
        this.ammo = ammo;

    }

    public void AddAmmo(int increment)
    {
        Ammo += increment;
    }

    public void SpendAmmo()
    {
        Ammo--;
    }

    public void UpgradeDamage(float increment)
    {
        damage += increment;
        ProjectileModel = new(damage, lifetime);
    }

    public RangedWeaponSaveData ToSaveData()
    {
        var data = new RangedWeaponSaveData();
        data.Damage = damage;
        data.Lifetime = lifetime;
        data.AttackCooldown = AttackCooldown;
        data.Ammo = ammo;
        return data;
    }

    public void FromSaveData(RangedWeaponSaveData data)
    {
        damage = data.Damage;
        lifetime = data.Lifetime;
        attackCooldown = data.AttackCooldown;
        ammo = data.Ammo;
    }
}