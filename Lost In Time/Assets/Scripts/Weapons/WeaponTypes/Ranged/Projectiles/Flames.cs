using UnityEngine;

public class Flames : Projectile
{
    [SerializeField] Vector2 startSize;
    [SerializeField] Vector2 endSize;
    private float sizeGrowTime = 1f; // Fixed
    private float lerpTime;
    protected override void OnEnable()
    {
        base.OnEnable();

        transform.localScale = startSize;
        lerpTime = 0;
    }
    protected override void Update()
    {
        base.Update();

        if (lerpTime < sizeGrowTime)
        {
            lerpTime += Time.deltaTime / travelTime;
            transform.localScale = Vector2.Lerp(startSize, endSize, lerpTime);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.tag == "Enemy")
        {
            ApplyKnockback(collision.GetComponent<Enemy>());
            DamageEnemy(collision.GetComponent<IDamageable>());
        }
    }

    protected override void DamageEnemy(IDamageable enemyDamageable)
    {
        if (enemyDamageable == null)
        {
            return;
        }

        enemyDamageable.TakeDamage(damage);
    }
}
