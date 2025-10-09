using System.Collections;
using UnityEngine;

public class HighNoon : Ranged
{
    private int bulletsLeft;
    private int maxBulletCapacity;
    private float reloadTime;

    public override void Fire()
    {
        if (bulletsLeft <= 0)
        {
            return;
        }

        base.Fire();
    }

    protected override void Awake()
    { 
        base.Awake();
        weaponType = WeaponCatalogue.Revolver;

        maxBulletCapacity = weaponData.numOfShots;
        bulletsLeft = maxBulletCapacity;
        reloadTime = weaponData.burstCooldown;
        numOfShots = 1;
    }

    protected override void SpawnBullet()
    {
        base.SpawnBullet();
        bulletsLeft -= 1;
    }

    protected override void Update()
    {
        if (bulletsLeft <= 0 && Time.time >= timeSinceLastFire + reloadTime && !isFiring)
        {
            canFire = true;
            bulletsLeft = maxBulletCapacity;
        }
        else if (bulletsLeft > 0 && Time.time >= timeSinceLastFire + cooldown && !isFiring)
        {
            canFire = true;
        }
    }
}
