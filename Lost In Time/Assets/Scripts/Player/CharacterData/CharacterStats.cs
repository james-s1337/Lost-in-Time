using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData charData;

    // Base
    public float baseHP;
    public float baseHPRegen;
    public float baseSpeed;
    public float baseJumpPower;
    public int baseJumps;
    public float baseDashForce;
    public int baseDashes;
    public float baseArmor; // 0 - 0.9
    public float baseDamage;
    public float abilityCooldown; // Not from data, always default of 0
    public float baseOvershield;

    // Dynamic
    [SerializeField] private float currentHP;
    [SerializeField] private float overshield;

    // On Touch
    public float infectiousTouch;
    public float burningTouch;
    public float freezingTouch;
    public float spikyTouch;
    public float stickyTouch;

    // Misc
    public float momentum;
    public float lastStand;
    public int falseCard;
    public int explodingGift;
    public int deadlyRush;

    // Other stats
    public float goldGoldGold; // Extra gold percentage
    public float extraExp; // Extra exp on kill

    private int exp;
    private int gold;
    private int level;
    private float statIncreasePerLevel = 0.2f;

    // Time stamps
    private float timeSinceLastRegen;
    private float timeSinceLastShieldRegen;
    private float timeSinceLastDamage;

    private float overshieldRegenStep = 0.5f; // Every 0.5 seconds
    private float overshieldRegenCooldown = 5f; // how much time you must wait before your overshield heals
    private float overshieldRegenAmount = 0.1f; // 10%

    private float HPRegenStep = 2f; // Every 2 seconds

    // Temp
    private float tempHPRegen;
    private float timeSinceTempHPRegen;
    private float tempHpRegenDuration;

    private float tempDamage;
    private float timeSinceTempDamage;
    private float tempDamageDuration;

    private float tempArmor;
    private float timeSinceTempArmor;
    private float tempArmorDuration;

    private float tempSpeed;
    private float timeSinceTempSpeed;
    private float tempSpeedDuration;

    // Lists
    private List<IStatModifier> activeModifiers = new List<IStatModifier>();

    private void Start()
    {
        level = 1;
        CalculateStats();
        currentHP = baseHP;
        overshield = baseOvershield;
    }

    private void Update()
    {
        if (Time.time >= timeSinceLastRegen + HPRegenStep)
        {
            RegenHP(baseHPRegen);
        }

        if (Time.time >= timeSinceLastDamage + overshieldRegenCooldown)
        {
            if (Time.time >= timeSinceLastShieldRegen + overshieldRegenStep)
            {
                RegenOvershield();
            }
        }

        CheckTempStatTimes();
    }

    public void RegenHP(float HPRegen)
    {
        float missingHealth = baseHP - currentHP;
        if (missingHealth == 0f)
        {
            return;
        }

        float regenAmount = HPRegen;
        if (regenAmount > missingHealth)
        {
            regenAmount = missingHealth;
        }

        timeSinceLastRegen = Time.time;
        currentHP += regenAmount;
    }

    public float GetCurrentHP()
    {
        return currentHP;
    }

    private void RegenOvershield()
    {
        float missingShield = baseOvershield - overshield;
        if (baseOvershield <= 0f || missingShield == 0f)
        {
            return;
        }

        float regenAmount = baseOvershield * overshieldRegenAmount;
        if (regenAmount > missingShield)
        {
            regenAmount = missingShield;
        }

        timeSinceLastShieldRegen = Time.time;
        overshield += regenAmount;
    }

    private void CheckTempStatTimes()
    {
        if (Time.time >= timeSinceTempHPRegen + tempHpRegenDuration)
        {
            tempHPRegen = 0;
            CalculateStats();
        }

        if (Time.time >= timeSinceTempArmor + tempArmorDuration)
        {
            tempArmor = 0;
            CalculateStats();
        }

        if (Time.time >= timeSinceTempDamage + tempDamageDuration)
        {
            tempDamage = 0;
            CalculateStats();
        }

        if (Time.time >= timeSinceTempSpeed + tempSpeedDuration)
        {
            tempSpeed = 0;
            CalculateStats();
        }
    }

    public void SetTimeSinceLastDamage()
    {
        timeSinceLastDamage = Time.time; 
    }

    public void AddModifier(IStatModifier modifier)
    {
        activeModifiers.Add(modifier);
        CalculateStats();
    }

    public void RemoveModifier(IStatModifier modifier)
    {
        activeModifiers.Remove(modifier);
        CalculateStats();
    }

    public void AddEXP(int amount)
    {
        exp += (int) (amount * extraExp);

        CheckLevelUp();
    }

    public void AddGold(int amount)
    {
        gold += (int) (amount * goldGoldGold);
    }

    public void RemoveGold(int amount)
    {
        if (amount > gold)
        {
            return;
        }

        gold -= amount;
    }

    public void TakeDamage(float amount)
    {
        amount -= amount * baseArmor;

        if (overshield > 0f)
        {
            overshield -= amount;
            overshield = Mathf.Clamp(overshield, 0, baseOvershield);
            return;
        }

        float newHP = currentHP - amount;
        if (currentHP - amount < 0f)
        {
            newHP = 0f;
        }

        currentHP = newHP;
    }

    public void CheckLevelUp()
    {
        int requiredExp = (level ^ 2) + (7 * level);
        if (exp > requiredExp)
        {
            level++;
            exp -= requiredExp;

            CheckLevelUp();
        }
    }

    public void CalculateStats()
    {
        ResetAllStats();
        SetBaseStatDefaults();

        foreach (var modifier in activeModifiers)
        {
            modifier.Apply(this);
        }

        if (currentHP > baseHP)
        {
            currentHP = baseHP;
        }

        if (overshield > baseOvershield)
        {
            overshield = baseOvershield;
        }
    }

    private void SetBaseStatDefaults()
    {
        baseHP = charData.health + (charData.health * statIncreasePerLevel * (level - 1));
        baseHPRegen = charData.healthRegen * baseHP / charData.health;
        baseHPRegen += baseHPRegen * tempHPRegen;
        baseSpeed = charData.movementSpeed + (charData.movementSpeed * tempSpeed);
        baseJumpPower = charData.jumpPower;
        baseJumps = charData.jumps;
        baseDashForce = charData.dashForce;
        baseDashes = charData.dashes;
        baseArmor = charData.armor + tempArmor;
        baseDamage = charData.baseDamage + tempDamage + (charData.baseDamage * statIncreasePerLevel * (level - 1));
    }

    public void ApplyTempStat(StatType mod, float duration, float amount)
    {
        switch (mod)
        {
            case StatType.MovementSpeed:
                tempSpeed = amount;
                timeSinceTempSpeed = Time.time;
                tempSpeedDuration = duration;
                CalculateStats();
                break;
            case StatType.Armor:
                tempArmor = amount;
                timeSinceTempArmor = Time.time;
                tempArmorDuration = duration;
                CalculateStats();
                break;
            case StatType.HealthRegen:
                tempHPRegen = amount;
                timeSinceTempHPRegen = Time.time;
                tempHpRegenDuration = duration;
                CalculateStats();
                break;
            default:
                tempDamage = amount;
                timeSinceTempDamage = Time.time;
                tempDamageDuration = duration;
                CalculateStats();
                break;
        }
    }

    private void ResetAllStats()
    {
        var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            if (field.FieldType == typeof(float))
                field.SetValue(this, 0f);
            else if (field.FieldType == typeof(int))
                field.SetValue(this, 0);
        }
    }
}