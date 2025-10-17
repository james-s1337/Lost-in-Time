using UnityEngine;

public class Reloadable : Ranged
{
    protected int bulletsLeft;

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

        ammo = weaponData.numOfShots;
        bulletsLeft = ammo;
        reloadTime = weaponData.burstCooldown;
        numOfShots = 1;
    }

    protected override void Update()
    {
        if (bulletsLeft <= 0 && Time.time >= timeSinceLastFire + reloadTime && !isFiring)
        {
            canFire = true;
            bulletsLeft = ammo;
        }
        else if (bulletsLeft > 0 && Time.time >= timeSinceLastFire + cooldown && !isFiring)
        {
            canFire = true;
        }
    }
}
