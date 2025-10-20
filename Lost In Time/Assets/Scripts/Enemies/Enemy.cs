using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IBurnable, IPoisonable, IFreezeable, ISlowable, IBleedable
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private string targetTag = "Player";

    public Core core { get; private set; }
    private BoxCollider2D boxCollider;

    private GameObject target;
    private Player player;

    private float movementSpeed;
    private float tempSpeed;
    private bool isFrozen;

    public float damage { get; private set; }
    public float maxHealth { get; private set; } 
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

    private void Update()
    {
        CheckStatusEffects();
    }

    private void FixedUpdate()
    {
        if (currentHealth <= 0 || isFrozen)
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

            CalculateSpeed();
            core.Movement.SetVelocity(movementSpeed, angle);
            core.Movement.CheckIfShouldFlip(direction);
        }
    }

    private void CalculateSpeed()
    {
        movementSpeed = enemyData.speed - (enemyData.speed * tempSpeed);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(damage);
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

    private void CheckStatusEffects()
    {
        for (int i = 0; i < statusEffects.Count; i++)
        {
            StatusEffectApplier effect = statusEffects[i];
            if (Time.time >= effect.startTime + effect.duration)
            {
                statusEffects.Remove(effect);

                if (effect.effect == StatusEffect.Slow)
                {
                    tempSpeed = 0;
                }
                else if (effect.effect == StatusEffect.Freeze)
                {
                    isFrozen = false;
                }

                continue;
            }

            if (effect.effect == StatusEffect.Slow)
            {
                tempSpeed = effect.damagePerTick;
                continue;
            }
            else if (effect.effect == StatusEffect.Freeze)
            {
                isFrozen = true;
                continue;
            }

            if (Time.time >= effect.timeSinceLastTick + effect.tickRate)
            {
                effect.timeSinceLastTick = Time.time;
                statusEffects[i] = effect;
                TakeDamage(effect.damagePerTick);
            }
        }
    }

    public void ApplyBurn()
    {
        Debug.Log("Lava chicken");
        float burnDamagePerTick = 0.08f;
        float burnTickRate = 1f;
        float burnDuration = 2f;
        
        StatusEffectApplier newBurn = new StatusEffectApplier();
        newBurn.effect = StatusEffect.Burn;
        newBurn.duration = burnDuration;
        newBurn.damagePerTick = maxHealth * burnDamagePerTick;
        newBurn.tickRate = burnTickRate;

        newBurn.startTime = Time.time;
        newBurn.timeSinceLastTick = Time.time;

        statusEffects.Add(newBurn);
    }

    public void ApplyPoison()
    {
        Debug.Log("COVID-19 Aqcuired.");
        float poisonDamagePerTick = 0.02f;
        float poisonTickRate = 0.5f;
        float poisonDuration = 4f;

        StatusEffectApplier newPoison = new StatusEffectApplier();
        newPoison.effect = StatusEffect.Poison;
        newPoison.duration = poisonDuration;
        newPoison.damagePerTick = maxHealth * poisonDamagePerTick;
        newPoison.tickRate = poisonTickRate;

        newPoison.startTime = Time.time;
        newPoison.timeSinceLastTick = Time.time;

        statusEffects.Add(newPoison);
    }

    public void ApplyFreeze()
    {
        Debug.Log("Let it goooo");
        float freezeDuration = 1.5f;

        StatusEffectApplier newFreeze = new StatusEffectApplier();
        newFreeze.effect = StatusEffect.Freeze;
        newFreeze.duration = freezeDuration;

        newFreeze.startTime = Time.time;

        statusEffects.Add(newFreeze);
    }

    public void ApplySlow()
    {
        Debug.Log("Turbo Reduced");
        float slowAmount = 0.5f;
        float slowDuration = 2f;

        StatusEffectApplier newSlow = new StatusEffectApplier();
        newSlow.effect = StatusEffect.Slow;
        newSlow.duration = slowDuration;
        newSlow.damagePerTick = slowAmount;

        newSlow.startTime = Time.time;

        statusEffects.Add(newSlow);
    }

    public void ApplyBleed()
    {
        Debug.Log("oof...oof");
        float bleedDamagePerTick = 0.04f;
        float bleedTickRate = 0.6f;
        float bleedDuration = 3f;

        StatusEffectApplier newBleed = new StatusEffectApplier();
        newBleed.effect = StatusEffect.Bleed;
        newBleed.duration = bleedDuration;
        newBleed.damagePerTick = maxHealth * bleedDamagePerTick;
        newBleed.tickRate = bleedTickRate;

        newBleed.startTime = Time.time;
        newBleed.timeSinceLastTick = Time.time;

        statusEffects.Add(newBleed);
    }
}

public struct StatusEffectApplier
{
    public StatusEffect effect;

    public float duration;
    public float damagePerTick;
    public float tickRate;

    public float startTime;
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
