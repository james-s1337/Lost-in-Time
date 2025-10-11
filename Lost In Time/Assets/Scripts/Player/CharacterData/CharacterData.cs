using UnityEngine;

[CreateAssetMenu(fileName = "newCharacterData", menuName = "Data/Character Data/Base Data")]
public class CharacterData : ScriptableObject
{
    [Header("Movement")]
    public float movementSpeed = 10.0f;
    public float jumpPower = 10.0f;
    public int jumps = 1;
    public float coyoteTime = 0.2f;
    public float jumpMult = 0.7f;
    public float defaultGravity = 2.5f;
    public float gravityFallMult = 2f;

    [Header("Vitals")]
    public int health = 50;
}
