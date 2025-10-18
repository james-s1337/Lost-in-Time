using System.Collections.Generic;
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
    public float baseArmor;
    public float baseDamage;
    public float abilityCooldown; // Not from data, always default of 0
    public float baseOvershield;

    // Dynamic
    public float currentHP;
    public float overshield;

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
    public int exp;
    public int gold;
    public int level;
    public float statIncreasePerLevel = 0.2f;

    // Time stamps
    private float timeSinceLastRegen;
    private float timeSinceLastShieldRegen;
    private float timeSinceLastDamage;

    private float overshieldRegenStep = 0.5f; // Every 0.5 seconds
    private float overshieldRegenCooldown = 5f; // how much time you must wait before your overshield heals
    private float overshieldRegenAmount = 0.1f; // 10%

    private float HPRegenStep = 2f; // Every 2 seconds

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
    }

    private void RegenHP(float HPRegen)
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
        exp += amount;

        CheckLevelUp();
    }

    public void AddGold(int amount)
    {
        gold += amount;
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
        baseHP = charData.health + (charData.health * statIncreasePerLevel * (level-1));
        baseHPRegen = charData.healthRegen + (charData.healthRegen * statIncreasePerLevel * (level - 1));
        baseSpeed = charData.movementSpeed;
        baseJumpPower = charData.jumpPower;
        baseJumps = charData.jumps;
        baseDashForce = charData.dashForce;
        baseDashes = charData.dashes;
        baseArmor = charData.armor;
        baseDamage = charData.baseDamage + (charData.baseDamage * statIncreasePerLevel * (level - 1));

        abilityCooldown = 0;
        infectiousTouch = 0;
        burningTouch = 0;
        freezingTouch = 0;
        spikyTouch = 0;
        stickyTouch = 0;
        falseCard = 0;
        explodingGift = 0;
        deadlyRush = 0;
        lastStand = 0;
        momentum = 0;

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
}