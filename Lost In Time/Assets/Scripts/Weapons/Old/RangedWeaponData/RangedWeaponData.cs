using UnityEngine;

[CreateAssetMenu(fileName = "newRangedWeaponData", menuName = "Data/Ranged Weapon Data/Base Data")]
public class RangedWeaponData : ScriptableObject
{
    [Header("PvE Stats")]
    public int damage = 2;
    public float cooldown = 0.3f;
    public float burstCooldown = 1f;
    public int numOfShots = 1;

    public int bounces = 0;
    public int piercing = 0;
    public float knockback = 0;
    public float recoil = 0;
    public float travelTime = 1;
    public float travelDistance;
    public float projectileSpeed = 25;
    public int critChance = 0;
    public int critDamage = 0;
}
