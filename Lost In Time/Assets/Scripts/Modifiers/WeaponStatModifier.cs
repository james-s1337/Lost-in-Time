using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/WeaponModifier")]
public class WeaponStatModifier : ScriptableObject, IWeaponModifier
{
    public List<WeaponMod> mods = new List<WeaponMod>();

    public void Apply(WeaponStats stats)
    {
        foreach (WeaponMod weaponMod in mods)
        {
            WeaponModifier mod = weaponMod.mod;
            float amount = weaponMod.amount;

            switch (mod)
            {
                case WeaponModifier.Damage: stats.baseDamage += stats.baseDamage * amount; break;
                case WeaponModifier.Cooldown: stats.baseCooldown = stats.baseCooldown / (amount + 1f); break;
                case WeaponModifier.Piercing: stats.piercing += (int)amount; break;
                case WeaponModifier.TravelTime: stats.baseTravelTime += amount; break;
                case WeaponModifier.TravelDistance: stats.baseTravelDistance += amount; break;
                case WeaponModifier.Knockback: stats.baseKnockback += amount; break;
                case WeaponModifier.ProjectileSpeed: stats.baseProjectileSpeed += amount; break;
                case WeaponModifier.CritChance: stats.baseCritChance += amount; break;
                case WeaponModifier.CritDamage: stats.baseCritDamage += amount; break;
                case WeaponModifier.ReloadSpeed: stats.baseReloadTime = stats.baseReloadTime / (amount + 1f); break;
                case WeaponModifier.Ammo: stats.ammo += (int)amount; break;

                case WeaponModifier.SpeedDMG: stats.speedDamageScaling += amount; break;

                case WeaponModifier.ArrowSpawn: stats.arrowSpawns += (int)amount; break;
                case WeaponModifier.PlasmaSpawn: stats.plasmaSpawns += (int)amount; break;
                case WeaponModifier.Waves: stats.waves += (int)amount; break;
                case WeaponModifier.Spikes: stats.spikes += (int)amount; break;
                case WeaponModifier.Drones: stats.drones += (int)amount; break;

                case WeaponModifier.BurnChance: stats.burnChance += amount; break;
                case WeaponModifier.PoisonChance: stats.poisonChance += amount; break;
                case WeaponModifier.FreezeChance: stats.freezeChance += amount; break;
                case WeaponModifier.BleedChance: stats.bleedChance += amount; break;
                case WeaponModifier.SlowChance: stats.slowChance += amount; break;
                case WeaponModifier.FireDMG: stats.fireDamage += amount; break;
                case WeaponModifier.PoisonDMG: stats.poisonDamage += amount; break;
                case WeaponModifier.FreezeDMG: stats.freezeDamage += amount; break;
                case WeaponModifier.BleedBMG: stats.bleedDamage += amount; break;
                case WeaponModifier.SlowDMG: stats.slowDamage += amount; break;

                case WeaponModifier.FullHPDMG: stats.fullHPDamage += amount; break;
                case WeaponModifier.NearDMG: stats.nearDamage += amount; break;
                case WeaponModifier.FarDMG: stats.farDamage += amount; break;
                case WeaponModifier.WeakDMG: stats.weakDamage += amount; break;
                case WeaponModifier.OverHeadDMG: stats.overheadDamage += amount; break;
                case WeaponModifier.Backstab: stats.backDamage += amount; break;
                case WeaponModifier.FirstStrike: stats.firstStrikeDamage += amount; break;
                case WeaponModifier.DashDamage: stats.afterDashDamage += amount; break;
                case WeaponModifier.StatueDMG: stats.statueDamage += amount; break;
                case WeaponModifier.InAirDMG: stats.inAirDamage += amount; break;
                case WeaponModifier.HitNRun: stats.hitAndRun += amount; break;

                case WeaponModifier.ProcGod: stats.doubleProc += (int)amount; break;
                case WeaponModifier.LifeSteal: stats.lifesteal += amount; break;
                case WeaponModifier.ExplosionChance: stats.explosionChance += amount; break;
                case WeaponModifier.LightningChance: stats.lightningChance += amount; break;
                case WeaponModifier.ExecuteEnemies: stats.executeThreshold += amount; break;

                case WeaponModifier.DamageBoostAfterKill: stats.damageBoostAfterKill += amount; break;
                case WeaponModifier.SpeedBoostAfterKill: stats.speedBoostAfterKill += amount; break;
                case WeaponModifier.ArmorAfterKill: stats.armorAfterKill += amount; break;
                case WeaponModifier.RegenAfterKill: stats.regenAfterKill += amount; break;
                case WeaponModifier.SoulDMG: stats.soulDamage += amount; break;
                case WeaponModifier.HealthOrb: stats.healthOrbAfterKill += amount; break;
                case WeaponModifier.Looting: stats.looting += amount; break;
                case WeaponModifier.Virus: stats.virus += amount; break;
                case WeaponModifier.Wildfire: stats.wildfire += amount; break;
                case WeaponModifier.Frostbite: stats.frostbite += amount; break;
                case WeaponModifier.Goo: stats.goo += amount; break;
                case WeaponModifier.Hemorrhage: stats.hemorrhage += amount; break;
                case WeaponModifier.Fuel: stats.fuel += amount; break;
                case WeaponModifier.Predator: stats.predator += (int) amount; break;
            }
        }
    }
}

public enum WeaponModifier
{
    // Perks
    // Weapon Stats
    Cooldown,
    Damage,
    Bounce,
    Piercing,
    TravelTime,
    TravelDistance,
    Knockback,
    Recoil,
    ProjectileSpeed,
    CritChance,
    CritDamage,
    ReloadSpeed,
    Ammo,
    SpeedDMG,
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
    DashDamage,
    StatueDMG,
    ExplosionChance,
    InAirDMG,
    HitNRun,
    LightningChance,
    ExecuteEnemies,
    // On Kill

    DamageBoostAfterKill,
    SpeedBoostAfterKill,
    ArmorAfterKill,
    RegenAfterKill,

    SoulDMG, // 0.1%+ per kill, stacks to 20% damage
    HealthOrb,
    Looting,
    Virus,
    Wildfire,
    Goo,
    Frostbite,
    Hemorrhage,
    Fuel, // Reduce ability cooldown on kill
    Predator, // After getting a kill with melee, do x2 damage for 3 seconds
}

[System.Serializable]
public struct WeaponMod
{
    public WeaponModifier mod;
    public float amount;
}
