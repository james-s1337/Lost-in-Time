using UnityEngine;

[CreateAssetMenu(fileName = "newCharacterData", menuName = "Data/Character Data/Base Data")]
public class CharacterData : ScriptableObject
{
    [Header("Movement")]
    public float movementSpeed = 10.0f;
    public float jumpPower = 10.0f;
    public int jumps = 1;
    public float dashForce = 15f;
    public int dashes = 1;

    // Const
    public float coyoteTime = 0.2f;
    public float jumpMult = 0.7f;
    public float defaultGravity = 2.5f;
    public float gravityFallMult = 2f;

    [Header("Vitals")]
    public float health = 50;
    public float healthRegen = 4; // Rate per 2 second
    public float armor = 0; // Damage reduction in percentage
    public float baseDamage = 1f; // Percentage
}
