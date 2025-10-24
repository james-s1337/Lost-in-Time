using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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
    public float fireRateBoostAfterKill;
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

    // Temp stats
    private float tempDamage;
    private float tempFireRate;

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
        SetBaseStatDefaults();

        foreach (var modifier in weaponModifiers)
        {
            modifier.Apply(this);
        }

        
    }

    private void SetBaseStatDefaults()
    {
        baseDamage = weaponData.damage * (tempDamage + 1f);
        baseCooldown = weaponData.cooldown / (tempFireRate + 1f);
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

    public IEnumerator ApplyTempStat(WeaponModifier mod, float duration, float amount)
    {
        switch (mod)
        {
            case WeaponModifier.Damage:
                tempDamage += amount;
                CalculateWeaponStats();
                yield return new WaitForSeconds(duration);
                tempDamage -= amount;
                CalculateWeaponStats();
                break;
            case WeaponModifier.Cooldown:
                tempFireRate += amount;
                CalculateWeaponStats();
                yield return new WaitForSeconds(duration);
                tempFireRate -= amount;
                CalculateWeaponStats();
                break;
        }
    }

    // Called when a projectile first hits an enemy
    public void ApplyEffects(GameObject hit, Vector2 startPos, Vector2 endPos)
    {
        if (hit == null)
        {
            return;
        }

        Enemy enemy = hit.GetComponent<Enemy>();
        Player player = GetComponentInParent<Player>();

        if (enemy == null || player == null)
        {
            return;
        }

        IDamageable enemyDamageable = enemy.GetComponent<IDamageable>();

        if (enemyDamageable == null)
        {
            return;
        }

        float totalDamage = 0;
        float scaledBaseDamage = baseDamage * player.characterStats.baseDamage;
        // Check if enemy is executable
        float enemyHealthPercentage = enemy.currentHealth / enemy.maxHealth;
        bool executed = false;
        if (enemyHealthPercentage <= executeThreshold)
        {
            totalDamage = enemy.currentHealth;
            executed = true;
        }

        if (!executed)
        {
            // StatType
            float lastStandDamageBoost = 1f;
            float lastStandThreshold = 0.2f;
            float lastStandDamage = 0;

            if (player.characterStats.GetCurrentHP() / player.characterStats.baseHP <= lastStandThreshold)
            {
                lastStandDamage = scaledBaseDamage * lastStandDamageBoost;
            }

            // Check if enemy is affected by any status
            float burnDMG = 0;
            float freezeDMG = 0;
            float poisonDMG = 0;
            float slowDMG = 0;
            float bleedDMG = 0;

            foreach (StatusEffectApplier effect in enemy.statusEffects)
            {
                if (effect.effect == StatusEffect.Burn)
                {
                    burnDMG = scaledBaseDamage * fireDamage;
                }
                else if (effect.effect == StatusEffect.Freeze)
                {
                    freezeDMG = scaledBaseDamage * freezeDamage;
                }
                else if (effect.effect == StatusEffect.Poison)
                {
                    poisonDMG = scaledBaseDamage * poisonDamage;
                }
                else if (effect.effect == StatusEffect.Slow)
                {
                    slowDMG = scaledBaseDamage * poisonDamage;
                }
                else if (effect.effect == StatusEffect.Bleed)
                {
                    bleedDMG = scaledBaseDamage * poisonDamage;
                }
            }

            // Other
            float maxFar = 20f;
            float maxNear = 2f;
            float distanceTravelled = (startPos - endPos).magnitude;

            float maxFarDamage = distanceTravelled / maxFar;
            maxFarDamage = Mathf.Clamp(maxFarDamage, 0f, 10f) * (scaledBaseDamage * farDamage);

            float maxNearDamage = maxNear / distanceTravelled;
            maxNearDamage = Mathf.Clamp(maxNearDamage, 0f, 10f) * (scaledBaseDamage * nearDamage);

            float fullHPDMG = 0;
            if (player.characterStats.GetCurrentHP() == player.characterStats.baseHP)
            {
                fullHPDMG = fullHPDamage * scaledBaseDamage;
            }

            float weakDamage = 0;
            float weakHealthThreshold = 0.3f;
            if (enemyHealthPercentage < weakHealthThreshold)
            {
                weakDamage = scaledBaseDamage * weakDamage;
            }

            float overheadDamage = 0;
            if (startPos.y > endPos.y && hit.transform.position.y < endPos.y)
            {
                overheadDamage = scaledBaseDamage * overheadDamage;
            }

            float backstabDamage = 0;
            if (Mathf.Abs(hit.transform.position.x + enemy.core.Movement.facingDir + endPos.x) > Mathf.Abs(hit.transform.position.x + endPos.x))
            {
                backstabDamage = scaledBaseDamage * backDamage;
            }

            float firstStrikeDamage = 0;
            float hitAndRunTime = 1.5f;
            if (enemy.currentHealth == enemy.maxHealth)
            {
                firstStrikeDamage = scaledBaseDamage * firstStrikeDamage;
                // Hit and run as well
                player.characterStats.ApplyTempStat(StatType.MovementSpeed, hitAndRunTime, hitAndRun);
            }

            // Dash damage: Check timeSinceLastDash, if <= 0.5f, then set dashDamage to something

            float statueDamage = 0;
            if (player.core.Movement.velocity.magnitude == 0f)
            {
                statueDamage = scaledBaseDamage * statueDamage;
            }

            float inAirDamage = 0;
            if (player.GetCurrentState() is PlayerInAir)
            {
                inAirDamage = scaledBaseDamage * inAirDamage;
            }

            float speedDamage = scaledBaseDamage * speedDamageScaling
                * (Mathf.Clamp(player.core.Movement.velocity.x - player.characterStats.charData.movementSpeed, 0f, 40f));

            float soulDMG = scaledBaseDamage * soulDamage;

            totalDamage = scaledBaseDamage + maxFarDamage + maxNearDamage + fullHPDMG + weakDamage
                + overheadDamage + backstabDamage + firstStrikeDamage + statueDamage + inAirDamage + soulDMG + burnDMG + freezeDMG
                + poisonDMG + slowDMG + bleedDMG + lastStandDamage;

            if (Random.value < baseCritChance)
            {
                totalDamage *= 2f + baseCritDamage;
            }
        }

        enemyDamageable.TakeDamage(totalDamage);

        // On-hit effects on player
        player.characterStats.RegenHP(scaledBaseDamage * lifesteal);

        // Proc
        if (enemy && enemy.currentHealth > 0)
        {
            if (Random.value < burnChance * (doubleProc + 1))
            {
                IBurnable status = enemy.GetComponent<IBurnable>();
                if (status != null)
                {
                    status.ApplyBurn();
                }
            }

            if (Random.value < poisonChance * (doubleProc + 1))
            {
                IPoisonable status = enemy.GetComponent<IPoisonable>();
                if (status != null)
                {
                    status.ApplyPoison();
                }
            }

            if (Random.value < freezeChance * (doubleProc + 1))
            {
                IFreezeable status = enemy.GetComponent<IFreezeable>();
                if (status != null)
                {
                    status.ApplyFreeze();
                }
            }

            if (Random.value < slowChance * (doubleProc + 1))
            {
                ISlowable status = enemy.GetComponent<ISlowable>();
                if (status != null)
                {
                    status.ApplySlow();
                }
            }

            if (Random.value < bleedChance * (doubleProc + 1))
            {
                IBleedable status = enemy.GetComponent<IBleedable>();
                if (status != null)
                {
                    status.ApplyBleed();
                }
            }
        }
        // After-effects
        // Spawn Lightning Object that uses an overlap and hitList to transfer to enemies
        // Spawn bomb that explodes onces and damage nearby enemies
        ApplyOnKillEffects(enemy, player);
    }

    private void ApplyOnKillEffects(Enemy enemy, Player player)
    {
        if (enemy.currentHealth > 0)
        {
            return;
        }
        float baseBoostTime = 2.5f;

        for (int i = 0; i < predator; i++)
        {
            float predatorBoostTime = 3f;
            float predatorDamageBoost = 2f;
            StartCoroutine(ApplyTempStat(WeaponModifier.Damage, predatorBoostTime, predatorDamageBoost));
        }

        StartCoroutine(ApplyTempStat(WeaponModifier.Damage, baseBoostTime, damageBoostAfterKill));
        StartCoroutine(ApplyTempStat(WeaponModifier.Cooldown, baseBoostTime, fireRateBoostAfterKill));
        player.characterStats.ApplyTempStat(StatType.MovementSpeed, baseBoostTime, speedBoostAfterKill);
        player.characterStats.ApplyTempStat(StatType.Armor, baseBoostTime, armorAfterKill);
        player.characterStats.ApplyTempStat(StatType.HealthRegen, baseBoostTime, regenAfterKill);



        float soulDamageAddAmount = 0.005f;
        for (int i = 0; i < weaponModifiers.Count; i++)
        {
            WeaponStatModifier weaponModifier = (WeaponStatModifier) weaponModifiers[i];
            for (int j = 0; j < weaponModifier.mods.Count; j++)
            {
                if (weaponModifier.mods[i].mod != WeaponModifier.SoulDMG)
                {
                    continue;
                }

                weaponModifier.mods[i].AddAmount(soulDamageAddAmount);
                break;
            }
        }
    }
}
