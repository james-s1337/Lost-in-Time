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

    protected Vector2 startPos;
    protected Vector2 endPos;

    protected WeaponStats weaponStats;

    private void Awake()
    {

    }
    private void Start()
    {
        // Play audio
    }

    protected virtual void OnEnable()
    {
        startPos = transform.position;
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

    public void SetWeaponStats(WeaponStats weaponStats)
    {
        if (this.weaponStats == null)
        {
            this.weaponStats = weaponStats;
        }
    }

    public void SetTravelTime(float travelTime)
    {
        this.travelTime = travelTime;
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
        endPos = transform.position;
    }

    protected virtual void ApplyKnockback(Enemy enemy)
    {
        enemy.core.Movement.AddForce(-(transform.position - enemy.gameObject.transform.position), knockback);
    }
}
