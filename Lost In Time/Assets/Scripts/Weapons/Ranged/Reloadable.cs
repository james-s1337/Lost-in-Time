using UnityEngine;

public class Reloadable : Ranged
{
    protected int bulletsLeft;
    protected int numOfShots;

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

        bulletsLeft = weaponStats.ammo;
        numOfShots = weaponStats.numOfShots;
    }

    protected override void Update()
    {
        if (bulletsLeft <= 0 && Time.time >= timeSinceLastFire + weaponStats.baseReloadTime && !isFiring)
        {
            canFire = true;
            bulletsLeft = weaponStats.ammo;
        }
        else if (bulletsLeft > 0 && Time.time >= timeSinceLastFire + weaponStats.baseCooldown && !isFiring)
        {
            canFire = true;
        }
    }
}
