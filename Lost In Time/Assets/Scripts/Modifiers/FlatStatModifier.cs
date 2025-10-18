using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/FlatStatModifier")]
public class FlatStatModifier : ScriptableObject, IStatModifier
{
    public StatType type;
    public float amount;
    public void Apply(CharacterStats stats)
    {
        switch (type)
        {
            case StatType.Health: stats.baseHP += amount; break;
            case StatType.HealthRegen: stats.baseHPRegen += amount; break;
            case StatType.MovementSpeed: stats.baseSpeed += amount; break;
            case StatType.JumpPower: stats.baseJumpPower += amount; break;
            case StatType.JumpCount: stats.baseJumps += (int) amount; break;
            case StatType.DashForce: stats.baseDashForce += amount; break;
            case StatType.DashCount: stats.baseDashes += (int) amount; break;
            case StatType.Armor: 
                stats.baseArmor += amount;
                stats.baseArmor = Mathf.Clamp(stats.baseArmor, 0f, 0.9f);
                break;
            case StatType.AbilityCooldown: stats.abilityCooldown += amount; break;
            case StatType.Overshield: stats.baseOvershield += amount; break;
            case StatType.LastStand: stats.lastStand += amount; break;
            case StatType.Momentum: stats.momentum += amount; break;
            case StatType.InfectiousTouch: stats.infectiousTouch += amount; break;
            case StatType.BurnTouch: stats.burningTouch += amount; break;
            case StatType.FreezingTouch: stats.freezingTouch += amount; break;
            case StatType.StickyTouch: stats.stickyTouch += amount; break;
            case StatType.SpikyTouch: stats.spikyTouch += amount; break;
            case StatType.FalseCard: stats.falseCard += (int) amount; break;
            case StatType.ExplodingGift: stats.explodingGift += (int) amount; break;
            case StatType.DeadlyRush: stats.deadlyRush += (int) amount; break;

            case StatType.GGG: stats.goldGoldGold += amount; break;
            case StatType.Exp: stats.extraExp += amount; break;
        }
    }
}

public enum StatType
{
    // Player Stats
    MovementSpeed,
    Armor, // Damage reduction
    JumpPower,
    JumpCount,
    DashCount,
    DashForce,
    Health,
    HealthRegen,

    AbilityCooldown,
    LastStand,
    Momentum, // Move faster the longer you dont get hit
    Overshield, // Extra healthbar, when damaging, will check if there is a value for this
    // When you get hit (in Player's OnTriggerEnter)
    InfectiousTouch,
    BurnTouch,
    FreezingTouch,
    SpikyTouch,
    StickyTouch,
    // Misc
    FalseCard, // Purchases are free, 50% chance to break after use
    ExplodingGift, // Dashing leaves bombs behind you
    DeadlyRush, // Dashing through enemies deals damage 
    GGG,
    Exp,
}
