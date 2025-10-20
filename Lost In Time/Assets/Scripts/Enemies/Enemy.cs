using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private string targetTag = "Player";

    public Core core { get; private set; }
    private BoxCollider2D boxCollider;

    private GameObject target;
    private Player player;

    private float movementSpeed;

    public float damage { get; private set; }
    public int maxHealth { get; private set; } 
    public float currentHealth { get; private set; }

    public List<StatusEffectApplier> statusEffects = new List<StatusEffectApplier>();

    private void Awake()
    {
        core = GetComponentInChildren<Core>();
        boxCollider = GetComponent<BoxCollider2D>();

        movementSpeed = enemyData.speed;
        maxHealth = enemyData.health;
        currentHealth = maxHealth;
        damage = enemyData.damage;
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag(targetTag);
        if (target)
        {
            player = target.GetComponent<Player>();
        }
    }

    private void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            return;
        }

        Chase();
        core.LogicUpdate();
    }

    void Chase()
    {
        if (core && target)
        {
            // Move towards player using movement core
            Vector2 angle = (target.transform.position - transform.position);
            int direction = 1;
            if (angle.x < 0)
            {
                direction = -1;
            }

            core.Movement.SetVelocity(movementSpeed, angle);
            core.Movement.CheckIfShouldFlip(direction);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            core.Movement.SetVelocityZero();
            StartCoroutine(Die());
        }  
    }

    private IEnumerator Die()
    {
        // Give player drops
        if (player)
        {

        }

        // Die effects
        boxCollider.enabled = false;

        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player)
        {
            player.core.Movement.AddForce(-(transform.position - player.gameObject.transform.position), enemyData.knockback);
            // DO Damage to player
            IDamageable playerDamageable = player.GetComponent<IDamageable>();
            if (playerDamageable == null)
            {
                return;
            }

            playerDamageable.TakeDamage(damage);
        }
    }
}

public struct StatusEffectApplier
{
    public StatusEffect effect;

    public float duration;
    public float damagePerTick;
    public float tickRate;

    public float timeSinceLastTick;

    public void AddDuration(float duration)
    {
        this.duration = duration;
    }
}

public enum StatusEffect
{
    Burn,
    Freeze,
    Poison,
    Slow,
    Bleed,
}
