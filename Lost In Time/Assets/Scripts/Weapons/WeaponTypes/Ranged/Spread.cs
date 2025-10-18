using System.Collections;
using UnityEngine;

public class Spread : Reloadable
{
    [SerializeField] private int pellets = 2;
    [SerializeField] private float spread = 15f; // Angle in between bullets
    private float currentSpread;
    protected override void Awake()
    {
        base.Awake();
        weaponType = WeaponCatalogue.Shotgun;

        ammo = pellets;
        bulletsLeft = ammo;
        reloadTime = weaponData.burstCooldown;
        numOfShots = weaponData.numOfShots;
    }

    public override void Fire()
    {
        if (!canFire || !weaponData || bulletsLeft <= 0)
        {
            return;
        }

        canFire = false;
        isFiring = true;

        SpawnBullet();
        SetTimeSinceLastFired();
    }

    protected override void SpawnBullet()
    {
        if (numOfShots % 2 == 0)
        {
            currentSpread = spread;
        }
        else
        {
            currentSpread = 0f;
        }

        for (int i = 0; i < numOfShots; i++)
        {
            StartCoroutine(SpawnPellet(currentSpread));

            if (currentSpread < 0 || currentSpread == 0f)
            {
                currentSpread = Mathf.Abs(currentSpread);
                currentSpread += spread;
            }
            else
            {
                currentSpread *= -1;
            }
        }
        bulletsLeft -= 1;
    }

    private IEnumerator SpawnPellet(float angleDeg)
    {
        int numOfPrefabs = bulletPrefabPool.Count;
        if (numOfPrefabs <= 0)
        {
            yield return null;
        }

        if (bulletIndex >= numOfPrefabs)
        {
            bulletIndex = 0;
        }

        GameObject bullet = bulletPrefabPool[bulletIndex];

        bullet.transform.position = transform.position;

        Projectile proj = bullet.GetComponent<Projectile>();
        proj.SetDamage(damage + speedDamage);
        proj.SetCritChance(critChance);
        proj.SetCritDamage(critDamage);
        proj.SetTravelTime(travelTime);
        proj.SetKnockback(knockback);
        proj.SetPiercing(pierce);
        proj.SetProjectileSpeed(projectileSpeed);

        if (player.core.Movement.facingDir == -1)
        {
            bullet.transform.Rotate(0f, 180f, 0f);
        }

        bullet.transform.Rotate(0f, 0f, angleDeg);

        bullet.SetActive(true);
        bulletIndex++;
        yield return null;
    }
}
