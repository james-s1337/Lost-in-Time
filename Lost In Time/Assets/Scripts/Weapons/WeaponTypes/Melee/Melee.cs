using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Melee : WeaponType
{
    // Melee Weapon Data
    [SerializeField] protected MeleeData weaponData;
    private BoxCollider2D collider;
    private List<Collider2D> hitList = new List<Collider2D>();

    private int damage;
    public override void Fire()
    {
        if (!canFire)
        {
            return;
        }

        base.Fire();
        Debug.Log("Swing");
        canFire = false;
        // Expand GameObject scale and hitbox based on size defined in Melee Weapon Data and modifiers
        // Play attack animation
        AnimationTrigger();
        AnimationFinishedTrigger();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();

        damage = weaponData.damage;
    }

    private void Start()
    {
        if (weaponData)
        {
            cooldown = weaponData.cooldown;
        }
    }

    void Update()
    {
        if (Time.time >= timeSinceLastFire + cooldown && !isFiring)
        {
            canFire = true;
        }
    }

    public void AnimationTrigger()
    {
        // Set hitbox to enabled
        collider.size = weaponData.size;
        collider.enabled = true;
    }
    public void AnimationFinishedTrigger()
    {
        // Disable hitbox
        collider.enabled = false;
        timeSinceLastFire = Time.time;
        isFiring = false;
        hitList.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !hitList.Contains(collision))
        {
            HitEnemy(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !hitList.Contains(collision))
        {
            HitEnemy(collision);
        }
    }

    private void HitEnemy(Collider2D collision)
    {
        hitList.Add(collision);
        // Do some damage
        IDamageable enemyDamageable = collision.GetComponent<IDamageable>();

        if (enemyDamageable != null)
        {
            enemyDamageable.TakeDamage(damage);
        }
    }
}
