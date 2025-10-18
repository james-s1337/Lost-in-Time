using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    protected float travelSpeed;
    protected float travelTime;
    protected float damage = 1;
    // Include audio for hit sound
    // Include audio for shoot sound
    protected Vector3 travelDirection;

    protected float critChance;
    protected float critDamage; // By default x2

    protected float knockback;
    protected int piercing;
    protected int targetsPierced;

    private void Awake()
    {
        
    }
    private void Start()
    {
        // Play audio
    }

    protected virtual void OnEnable()
    {
        Invoke(nameof(DisableBullet), travelTime);
    }

    public void Initialize()
    {
        DisableBullet();
    }

    protected virtual void DisableBullet()
    {
        targetsPierced = 0;
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
    }
    protected virtual void Update()
    {
        // transform.Translate(travelDirection * Time.deltaTime * travelSpeed, Space.World);
        transform.Translate(new Vector2(travelSpeed * Time.deltaTime, 0f));
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetTravelTime(float travelTime)
    {
        this.travelTime = travelTime;
    }

    public void SetCritChance(float critChance)
    {
        this.critChance = critChance;
    }

    public void SetCritDamage(float critDamage)
    {
        this.critDamage = critDamage;
    }

    public void SetKnockback(float knockback)
    {
        this.knockback = knockback;
    }

    public void SetPiercing(int piercing)
    {
        this.piercing = piercing;
    }

    public void SetProjectileSpeed(float travelSpeed)
    {
        this.travelSpeed = travelSpeed;
    }

    public virtual void SetFacingAngle(float angle)
    {
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    public virtual void SetTravelDirection(Vector3 direction)
    {
        travelDirection = direction.normalized;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // CHECK ON HIT EFFECTS
        if (Random.value < critChance)
        {
            damage *= 2;
            damage += (damage * critDamage);
        }
    }

    protected virtual void ApplyKnockback(Enemy enemy)
    {
        enemy.core.Movement.AddForce(-(transform.position - enemy.gameObject.transform.position), knockback);
    }

    protected virtual void DamageEnemy(IDamageable enemyDamageable)
    {
        if (enemyDamageable == null)
        {
            return;
        }

        enemyDamageable.TakeDamage(damage);

        targetsPierced++;
        if (targetsPierced < piercing)
        {
            return;
        }

        DisableBullet();
    }
}
