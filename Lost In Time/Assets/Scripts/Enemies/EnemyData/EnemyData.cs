using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyData", menuName = "Data/Enemy Data/Base Data")]
public class EnemyData : ScriptableObject
{
    [Header("Base Stats")]
    public int damage = 5;
    public int defense = 0;
    public float speed = 5f;
    public bool canFly = true;
    public bool canJump = false;

    public float knockback = 20f;

    [Header("Vitals")]
    public int health = 4;
}
