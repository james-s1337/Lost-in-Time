using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("PvE Stats")]
    public float damage = 2;
    public float cooldown = 0.3f;
    public float reloadTime = 1f;

    public int ammo;
    public int numOfShots = 1;
    public int piercing = 0;

    public float knockback = 0;
    public float recoil = 0;
    public float travelTime = 1;
    public float travelDistance;
    public float projectileSpeed = 25;
    public float critChance = 0;
    public float critDamage = 0;

    public float burnChance = 0;
    public float freezeChance = 0;
}
