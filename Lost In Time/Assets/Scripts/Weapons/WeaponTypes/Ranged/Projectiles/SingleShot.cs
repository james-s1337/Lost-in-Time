using UnityEngine;

public class SingleShot : Projectile
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.tag == "Enemy")
        {
            IDamageable enemyDamageable = collision.GetComponent<IDamageable>();
            if (enemyDamageable != null)
            {
                enemyDamageable.TakeDamage(damage);
                DisableBullet();
            }
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}
