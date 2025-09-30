using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Mine : Projectile
{
    [SerializeField] float explosionRadius;
    [SerializeField] LayerMask whatIsEnemy;
    private Rigidbody2D rb;

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
        base.OnEnable();
    }

    protected override void DisableBullet()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        StartCoroutine(DoAOEDamage());
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
                // Get IDamageable
                // If IDamageable, DamageEnemy()
            }
        }
        // Play explosion animation for mine sprite
        yield return new WaitForSeconds(0.3f);
        base.DisableBullet();
        
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            return;
        }

        // Check collision with enemy only
        if (collision.tag == "Enemy")
        {
            // DisableBullet()
        }

        if (collision.tag == "Ground")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
