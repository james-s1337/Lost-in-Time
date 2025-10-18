using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Mine : Projectile
{
    [SerializeField] float explosionRadius;
    [SerializeField] LayerMask whatIsEnemy;
    private Rigidbody2D rb;

    private bool isExploding;

    protected override void Update()
    {
        
    }

    protected override void OnEnable()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.constraints = rb.constraints & ~RigidbodyConstraints2D.FreezePositionX & ~RigidbodyConstraints2D.FreezePositionY;
        isExploding = false;

        base.OnEnable();
    }

    protected override void DisableBullet()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        isExploding = true;
        StartCoroutine(DoAOEDamage());
    }

    public override void SetFacingAngle(float angle)
    {
        
    }

    private IEnumerator DoAOEDamage()
    {
        Collider2D [] targets = Physics2D.OverlapCircleAll(transform.position, explosionRadius, whatIsEnemy);
        // Play sound
        // Play particles
        foreach (Collider2D target in targets)
        {
            if (target.tag == "Enemy")
            {
                ApplyKnockback(target.GetComponent<Enemy>());
                DamageEnemy(target.GetComponent<IDamageable>());
            }
        }
        // Play explosion animation for mine sprite
        yield return new WaitForSeconds(0.3f);
        base.DisableBullet();
        
    }

    protected override void DamageEnemy(IDamageable enemyDamageable)
    {
        if (enemyDamageable == null)
        {
            return;
        }

        enemyDamageable.TakeDamage(damage);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (isExploding)
        {
            return;
        }

        // Check collision with enemy only
        if (collision.tag == "Enemy")
        {
            DisableBullet();
        }

        if (collision.tag == "Ground")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
