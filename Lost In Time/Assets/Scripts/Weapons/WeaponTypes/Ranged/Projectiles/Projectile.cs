using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float travelSpeed;
    [SerializeField] protected float travelTime;
    protected int damage = 1;
    // Include audio for hit sound
    // Include audio for shoot sound
    protected Vector3 travelDirection;

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
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
    }
    protected virtual void Update()
    {
        // transform.Translate(travelDirection * Time.deltaTime * travelSpeed, Space.World);
        transform.Translate(new Vector2(travelSpeed * Time.deltaTime, 0f));
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetTravelTime(float travelTime)
    {
        this.travelTime = travelTime;
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
        if (collision.tag == "Player")
        {
            return;
        }

        //Check collision of enemy/obstacle
    }

    protected virtual void DamageEnemy(IDamageable enemyDamageable)
    {
        if (enemyDamageable == null)
        {
            return;
        }

        enemyDamageable.TakeDamage(damage);
        DisableBullet();
    }
}
