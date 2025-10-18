using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public WeaponData weaponData;

    // Base weapon stats
    public float baseDamage;
    public float baseCooldown;
    public float baseReloadTime;
    public float baseKnockback;
    public float baseTravelTime;
    public float baseTravelDistance;
    public float baseProjectileSpeed;
    public float baseCritChance;
    public float baseCritDamage;

    public int numOfShots;
    public int ammo;
    public int piercing;

    // Spawn after shooting
    public int arrowSpawns;
    public int plasmaSpawns;
    public int waves;
    public int spikes;
    public int drones;

    // Scaling with other stats
    public float speedDamageScaling;

    // Proc on-hit
    public float burnChance;
    public float poisonChance;
    public float freezeChance;
    public float bleedChance;
    public float slowChance;
    public float armorReduction; // Permanent (except for bosses)

    // Conditional on-hit
    public float fireDamage;
    public float poisonDamage;
    public float freezeDamage;
    public float bleedDamage;
    public float slowDamage;

    public float fullHPDamage;
    public float nearDamage;
    public float farDamage;
    public float weakDamage;
    public float overheadDamage;
    public float backDamage;
    public float firstStrikeDamage;
    public float afterDashDamage; // Bonus damage right after dash
    public float statueDamage; // Bonus damage while standing still
    public float inAirDamage;
    public float hitAndRun; // Hitting an enemy for the first time gives you a speed boost (speed boost percentage)

    // Additional on-hit
    public int doubleProc;
    public float lifesteal;
    public float explosionChance; // Chance to explode on-hit (explosions do 50% damage)
    public float lightningChance; // Chance to spawn chain lightning (lightning does 50% damage)
    public float executeThreshold; // % of health required to execute normal enemies

    // On kill
    public float damageBoostAfterKill;
    public float speedBoostAfterKill;
    public float armorAfterKill;
    public float regenAfterKill;
    public float soulDamage; // percentage, increases a little each kill
    public float healthOrbAfterKill; // Chance to drop a health orb
    public float looting; // Extra chance to drop a time capsule

    public float virus; // Chance to spread poison to nearby enemies;
    public float wildfire; // Chance to spread burn
    public float frostbite; // Chance for freezing nearby enemies on kill
    public float goo; // Chance for goo around killed enemy, slowing nearby enemies
    public float hemorrhage; // Chance to spread bleed on kill

    public float fuel; // Cooldown reduction on kill
    public int predator; // After getting a kill with melee, do x2 damage for 3 seconds

    private List<IWeaponModifier> weaponModifiers = new List<IWeaponModifier>();

    private void Start()
    {
        CalculateWeaponStats();
    }

    public void AddModifier(IWeaponModifier modifier)
    {
        weaponModifiers.Add(modifier);
        CalculateWeaponStats();
    }

    public void RemoveModifier(IWeaponModifier modifier)
    {
        weaponModifiers.Remove(modifier);
        CalculateWeaponStats();
    }

    public void CalculateWeaponStats()
    {
        ResetAllStats();

        baseDamage = weaponData.damage;
        baseCooldown = weaponData.cooldown;
        baseReloadTime = weaponData.reloadTime;
        baseKnockback = weaponData.knockback;
        baseTravelTime = weaponData.travelTime;
        baseTravelDistance = weaponData.travelDistance;
        baseProjectileSpeed = weaponData.projectileSpeed;
        baseCritChance = weaponData.critChance;
        baseCritDamage = weaponData.critDamage;
        ammo = weaponData.ammo;
        numOfShots = weaponData.numOfShots;
        piercing = weaponData.piercing;

        foreach (var modifier in weaponModifiers)
        {
            modifier.Apply(this);
        }
    }

    private void ResetAllStats()
    {
        var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
        foreach (var field in fields)
        {
            if (field.FieldType == typeof(float))
                field.SetValue(this, 0f);
            else if (field.FieldType == typeof(int))
                field.SetValue(this, 0);
        }
    }
}
