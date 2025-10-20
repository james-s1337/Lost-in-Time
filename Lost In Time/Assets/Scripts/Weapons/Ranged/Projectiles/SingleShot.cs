using UnityEngine;

public class SingleShot : Projectile
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.tag == "Enemy")
        {
            ApplyKnockback(collision.GetComponent<Enemy>());
            weaponStats.ApplyEffects(collision.gameObject, startPos, endPos);

            targetsPierced++;
            if (targetsPierced < piercing)
            {
                return;
            }

            DisableBullet();
        }
    }
}
