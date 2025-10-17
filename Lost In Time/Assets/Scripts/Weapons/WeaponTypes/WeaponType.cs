using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WeaponType : MonoBehaviour
{
    protected WeaponCatalogue weaponType;
    protected bool canFire;
    protected float cooldown;
    protected bool isFiring;
    protected float timeSinceLastFire;
    protected Player player;

    protected Dictionary<WeaponModifier, int> weaponModifiers = Enum.GetValues(typeof(WeaponModifier)).Cast<WeaponModifier>().ToDictionary(e => e, e => 0);
    protected List<WeaponPerk> weaponPerks = new List<WeaponPerk>();

    public virtual void Fire() { }  

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public WeaponCatalogue GetWeaponType()
    {
        return weaponType;
    }

    public void AddPerk(int perkIndex, WeaponModifier weapMod, int value)
    {
        RemovePerk(perkIndex, weapMod, weaponPerks[perkIndex].value);
        weaponPerks[perkIndex].ChangePerk(weapMod, value);
        weaponModifiers[weapMod] += value;
    }

    public void RemovePerk(int perkIndex, WeaponModifier weapMod, int value)
    {
        weaponPerks[perkIndex].SetZero();
        weaponModifiers[weapMod] -= value;
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
    Shotgun,
    Crossbow,
    Longsword,
    Spear,
}

public enum WeaponModifier
{
    // Perks
    Cooldown,
    Damage,
    Bounce,
    Piercing,
    TravelDistance,
    Knockback,
    ProjectileSpeed,
    CritChance,
    CritDamage,
    ArrowSpawn,
    PlasmaSpawn,
    ReloadSpeed,
    LifeSteal,
    MovementSpeed,
    FireDMG,
    PoisonDMG,
    FreezeDMG,
    BleedBMG,
    SlowDMG,
    BurnChance,
    PoisonChance,
    FreezeChance,
    BleedChance,
    SlowChance,
    FullHPDMG,
    Adrenaline,
    LastStand,
    Ammo,
    Deflect,
    Goo,
    Frostbite,
    NearDMG,
    FarDMG,
    WeakDMG,
    GGG, // GOLD GOLD GOlD
    ExpGain,
    OverHeadDMG,
    Backstab,
    InfectiousTouch,
    BurnTouch,
    FreezingTouch,
    SpikyTouch,
    StickyTouch,
    Basic,
    ProcGod,

    // Items (not included in perk section)
    FirstStrike,
    Armor, // Damage reduction
    DashDamage, // Bonus damage after dash
    JumpPower,
    JumpCount,
    DashCount,
    Health,
    HealthRegen,
    Triumph, // After getting a kill, gain resistance and hp regen 
    SoulDMG, // 0.1%+ per kill, stacks to 20% damage
    StatueDMG,
    HealthOrb,
    Momentum,
    ArmorReduction,
    StatusDamage,
    Looting,
    FalseCard,
    Waves,
    ExplodingDamage,
    SpeedDMG, // How much speed will convert to damage
    Spikes,
    InAirDMG,
    HitNRun,
    Virus,
    Fuel,
    Lightning,
    AbilityCooldown,
    ExecuteEnemies,
    Predator, // After getting a kill with melee, do x2 damage for 3 seconds
    ExplodingGift, // Dashing leaves bombs behind you
    Drones,
    Overshield,
    DeadlyRush, // Dashing through enemies deals damage
}

public struct WeaponPerk
{
    public WeaponModifier modifier;
    public int value;

    public void ChangePerk(WeaponModifier modifier, int value)
    {
        this.modifier = modifier;
        this.value = value;
    }

    public void SetZero()
    {
        value = 0;
    }
}
