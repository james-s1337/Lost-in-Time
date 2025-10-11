using System.Collections.Generic;
using UnityEngine;

public class WeaponType : MonoBehaviour
{
    protected WeaponCatalogue weaponType;
    protected bool canFire;
    protected float cooldown;
    protected bool isFiring;
    protected float timeSinceLastFire;
    protected Player player;

    protected Dictionary<WeaponModifier, int> weaponModifiers = new Dictionary<WeaponModifier, int>
    {
        { WeaponModifier.Cooldown, 0 }, // Percentage
        { WeaponModifier.Damage, 0 }, // Percentage
        { WeaponModifier.Projectiles, 0 }, // Ranged only
        { WeaponModifier.Piercing, 0 }, // Number of enemies
        { WeaponModifier.TravelDistance, 1 }, // Good for Flamethrower and Boomerang
        { WeaponModifier.Knockback, 0 }, // Extra force
        { WeaponModifier.ProjectileSpeed, 0 }, // Good for Flamethrower and Boomerang
        { WeaponModifier.CritChance, 0 }, // Percentage
        { WeaponModifier.CritDamage, 0 }, // Percentage
        { WeaponModifier.Size, 0 }, // Percentage
        { WeaponModifier.ReloadSpeed, 0 }, // Revolver only
        { WeaponModifier.LifeSteal, 0 },
    };
    public virtual void Fire() { }  

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public WeaponCatalogue GetWeaponType()
    {
        return weaponType;
    }
}

public enum WeaponCatalogue
{
    Pistol,
    Burst,
    Mine,
    Boomerang,
    Revolver,
    Flamethrower,
    Flintlock,
    Crossbow,
    Longsword,
    Spear,
}

public enum WeaponModifier
{
    Cooldown,
    Damage,
    Projectiles,
    Piercing,
    TravelDistance,
    Knockback,
    ProjectileSpeed,
    CritChance,
    CritDamage,
    Size,
    ReloadSpeed,
    LifeSteal,
}
