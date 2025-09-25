using UnityEngine;

public class SingleShot : Projectile
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void Update()
    {
        base.Update();
    }
}
