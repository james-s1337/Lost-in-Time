using UnityEngine;

[CreateAssetMenu(fileName = "newRangedWeaponData", menuName = "Data/Ranged Weapon Data/Base Data")]
public class RangedWeaponData : ScriptableObject
{
    [Header("PvE Stats")]
    public int damage = 2;
    public float cooldown = 0.3f;
    public float burstCooldown = 1f;
    public int numOfShots = 1;
    // Add status effect later
    public GameObject bulletPrefab;
}
