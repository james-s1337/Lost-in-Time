using UnityEngine;

public class SingleShot : Projectile
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.tag == "Enemy")
        {
            DamageEnemy(collision.GetComponent<IDamageable>());
        }
    }
}
