using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeWeaponData", menuName = "Data/Melee Weapon Data/Base Data")]
public class MeleeData : ScriptableObject
{
    [Header("PvE Stats")]
    public int damage = 2;
    public float cooldown = 0.5f;
    public Vector2 size = new Vector2(3f, 3f); // Reach

}
