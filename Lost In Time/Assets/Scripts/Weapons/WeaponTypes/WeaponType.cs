using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponType : MonoBehaviour
{
    protected WeaponCatalogue weaponType;
    protected bool canFire;
    protected bool isFiring;
    protected float timeSinceLastFire;
    protected Player player;
    protected CharacterData charData;

    protected float cooldown;
    protected int damage;
    protected int bounce;
    protected int pierce;
    protected float travelTime;
    protected float travelDistance; // Only for boomerang
    protected float knockback;
    protected float recoil;
    protected float projectileSpeed;
    protected int critChance;
    protected int critDamage;
    protected float reloadTime;
    protected int ammo;
    protected int speedDamage;

    public Dictionary<WeaponModifier, int> weaponModifiers {get; private set;}
    protected WeaponPerk[] weaponPerks = new WeaponPerk[3];
    protected virtual void Awake()
    {
        weaponModifiers = Enum.GetValues(typeof(WeaponModifier)).Cast<WeaponModifier>().ToDictionary(e => e, e => 0);
    }
    public virtual void Fire() { }  

    public void SetPlayer(Player player, CharacterData charData)
    {
        this.player = player;
        this.charData = charData;
    }

    public WeaponCatalogue GetWeaponType()
    {
        return weaponType;
    }

    public virtual void AddPerk(int perkIndex, WeaponModifier weapMod, int value)
    {
        RemovePerk(perkIndex);
        weaponPerks[perkIndex].ChangePerk(weapMod, value);

        weaponModifiers[weapMod] += value;
    }

    public virtual WeaponModifier RemovePerk(int perkIndex)
    {
        WeaponModifier mod = weaponPerks[perkIndex].GetModifier();
        weaponModifiers[mod] -= weaponPerks[perkIndex].GetValue();
        weaponPerks[perkIndex].SetZero();

        return mod;
    }
}

public enum WeaponCatalogue
{
    Pistol,
    EnergyPistol,
    Burst,
    Mine,
    Boomerang,
    Revolver,
    Flamethrower,
    Flintlock,
    Shotgun,
    Crossbow,
    Longsword,
    Katana,
    Spear,
    Dagger,
    Khopesh,
    Cutlass,
    Sniper,
    Cannon,
    IceGun,
}

public enum WeaponModifier
{
    // Perks
    // Weapon Stats
    Cooldown,
    Damage,
    Bounce,
    Piercing,
    TravelDistance,
    Knockback,
    Recoil,
    ProjectileSpeed,
    CritChance,
    CritDamage,
    ReloadSpeed,
    Ammo,
    SpeedDMG, // How much speed will convert to damage
    // Spawn
    ArrowSpawn,
    PlasmaSpawn,

    Waves,
    Spikes,
    Drones,
    // On-hit
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
    NearDMG,
    FarDMG,
    WeakDMG,
    OverHeadDMG,
    Backstab,
    Basic,
    ProcGod,
    LifeSteal,

    FirstStrike,
    ArmorReduction,
    DashDamage, // Bonus damage after dash
    StatueDMG,
    StatusDMG,
    ExplodingDMG,
    InAirDMG,
    HitNRun, // Hitting an enemy for the first time gives you a speed boost
    Lightning, // Chain lightning
    ExecuteEnemies,
    // On Kill
    Adrenaline,
    Deflect,
    Goo,
    Frostbite,
    GGG, // GOLD GOLD GOlD
    ExpGain,

    Triumph, // After getting a kill, gain resistance and hp regen 
    SoulDMG, // 0.1%+ per kill, stacks to 20% damage
    HealthOrb,
    Looting,
    Virus,
    Fuel, // Reduce ability cooldown on kill
    Predator, // After getting a kill with melee, do x2 damage for 3 seconds
}  

public struct WeaponPerk
{
    private WeaponModifier modifier;
    private int value;

    public void ChangePerk(WeaponModifier modifier, int value)
    {
        this.modifier = modifier;
        this.value = value;
    }

    public void SetZero()
    {
        value = 0;
    }

    public WeaponModifier GetModifier()
    {
        return modifier;
    }

    public int GetValue()
    {
        return value;
    }
}
